<script setup>
import { ref, toRaw } from 'vue';

import { closeGame, getGameById, saveGame, shareGame, getGameUid } from "../assets/js/game-api";
import QuizEditorQuestion from '../components/editor/QuizEditorQuestion.vue';
import ModalWindow from '../components/ModalWindow.vue'
import ShareWindow from '../components/ShareWindow.vue';
import { parseError } from '../assets/js/utilities';

const props = defineProps({
    paramId: {
        type: Number,
        required: false,
        default: -1
    }
});

const readOnly = ref(props.paramId != -1 ? true : false);

const taskTitle = ref("Unnamed"), taskDescription = ref("");

const questions = ref([]);
const loaded = ref(!readOnly.value);

let id = 0;

const addQuestion = () => {
    questions.value.push({ id: id++, description: "", maxGrade: 1.0, type: "SINGLE", answers: [] });
};

if (!readOnly.value) addQuestion();

const updateQuestion = (question) => {
    const index = questions.value.findIndex(item => item.id == question.id);
    if (index != -1) {
        question.answers = toRaw(question.answers);
        questions.value[index] = question;
    }

};

const modalText = ref(""), modalSubmit = ref(() => { }), modalCancel = ref(() => { });

const closeModal = () => {
    modalText.value = "";

    modalSubmit.value = () => { };
    modalCancel.value = () => { };
};

const removeQuestion = (number, text) => {
    modalText.value = `Do you really want to remove question #${number + 1} ${text}?`;

    modalSubmit.value = () => {
        questions.value.splice(number, 1);

        if (questions.value.length == 0) addQuestion();

        closeModal();
    };

    modalCancel.value = closeModal;
};

const errorMessage = ref("");

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err) => {
    active.value = false;
    console.log(err);

    errorMessage.value = parseError(err);
};

const validateData = (task) => {
    errorMessage.value = "";

    if (task.title.length == 0) {
        errorMessage.value = "Title is empty!";
        return false;
    }
    if (task.description.length == 0) {
        errorMessage.value = "Description is empty!";
        return false;
    }

    for (let i in task.questions) {
        const question = task.questions[i];

        if (question.description.length == 0) {
            errorMessage.value = `Question ${parseInt(i) + 1} description is empty!`;
            return false;
        }

        let isCorrect = false;

        for (let j in question.answers) {
            const answer = question.answers[j];

            if (answer.option.length == 0) {
                errorMessage.value = `Answer ${parseInt(i) + 1} option in question '${question.description}'' is empty!`;
                return false;
            }
            isCorrect ||= answer.correct;
        }

        if (!isCorrect && question.answers.length > 0) {
            errorMessage.value = `There must be at least one correct answer in question '${question.description}''!`;
            return false;
        }
    }

    return true;
}

const extractTask = () => {
    const questionsValue = toRaw(questions.value);

    for (let i in questionsValue) {
        questionsValue[i].description = questionsValue[i].description.trim();
        questionsValue[i]["max_grade"] = questionsValue[i].maxGrade;

        if (questionsValue[i].type == "TEXT") {
            questionsValue[i].answers = [];
        }
        for (let j in questionsValue[i].answers) {
            questionsValue[i].answers[j].option = questionsValue[i].answers[j].option.trim();
        }
    }

    const task = {
        title: toRaw(taskTitle.value).trim(),
        description: toRaw(taskDescription.value).trim(),
        questions: questionsValue
    };

    return task;
};

const active = ref(false);

const shareTask = () => {
    active.value = true;
};

const closeSharing = () => {
    active.value = false;
};

const game = ref({type:"SINGLEANSWER", "player_weapon":"SWORD", "enemy_weapon":"SWORD", "min_win_grade": 0});

const submitTask = () => {
    const task = extractTask();
    if (!validateData(task)) return;

    const gameObj = toRaw(game.value);
    gameObj.task = task;

    modalText.value = "After saving game you can't edit it anymore!";

    modalSubmit.value = () => {
        saveGame(gameObj)
            .then(res => {
                readOnly.value = true;
                window.location = `/game/editor/${res.id}`;
            })
            .catch(errorLog);
        closeModal();
    };

    modalCancel.value = closeModal;
};

if (readOnly.value) {
    getGameById(props.paramId).then(res => {

        game.value.type = res.type;
        game.value["player_weapon"] = res["player_weapon"];
        game.value["enemy_weapon"] = res["enemy_weapon"];
        game.value["min_win_grade"] = res["min_win_grade"];

        taskTitle.value = res.task.title;
        taskDescription.value = res.task.description;

        questions.value = res.task.questions.map(item => {
            const newItem = JSON.parse(JSON.stringify(item));
            newItem.maxGrade = item["max_grade"];
            return newItem;
        });

        loaded.value = true;
    }).catch(errorLog);
}

const showAnswers = () => {
    window.location = `/game/answers/${props.paramId}`;
};

</script>
<template>
    <section class="book_section mb-4">
        <ModalWindow :question="modalText" :submit="modalSubmit" :cancel="modalCancel"></ModalWindow>
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>

        <ShareWindow :active="active" :obj-id="paramId" :close="closeSharing" :error-log="errorLog"
        :get-obj="getGameUid" :share-obj="shareGame" :close-obj="closeGame" name="game"></ShareWindow>

        <form id="task-form" @submit.prevent="submitTask" onsubmit="return false;" style="background: none; box-shadow: none;">
            <div class="container" v-if="loaded">
                <div class="row">
                    <div class="col">

                        <form onsubmit="return false;">
                            <div>
                                <h4 class="form-row">
                                    <div class="form-group col-lg-2">
                                        <label>Game title</label>
                                    </div>
                                    <div class="form-group col-lg-6">
                                        <input type="text" class="form-control" placeholder="Game title..."
                                            v-model="taskTitle" required v-bind:readonly="readOnly" form="task-form">

                                    </div>
                                </h4>
                                <div class="share">
                                    <div>
                                        <button v-if="!readOnly" type="submit" class="btn btn-success mr-4"
                                            form="task-form">Save</button>
                                        <div v-else>
                                            <button type="button" class="btn btn-success mr-3"
                                                @click="showAnswers">Answers</button>
                                            <button type="button" class="btn btn-warning" @click="shareTask" >Share</button>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-row">
                                <div class="form-group col-lg-2">
                                    <label> Game type</label>
                                </div>
                                <div class="form-group col-lg-6">
                                    <select class="form-control" required v-model="game.type">
                                        <option value="SINGLEANSWER">Single answer game</option>
                                    </select>
                                </div>
                                <div class="form-group col-lg-6">
                                    <label>Player weapon</label>
                                    <select class="form-control" required v-model="game['player_weapon']">
                                        <option value="SWORD">Sword</option>
                                        <option value="HAMMER">Hammer</option>
                                        <option value="SCYTHE">Scythe</option>
                                    </select>
                                </div>
                                <div class="form-group col-lg-6 text-center">
                                    <img v-if="game['player_weapon'] == 'SWORD'" src="../assets/images/SWORD.png" class="weapon-img">
                                    <img v-else-if="game['player_weapon'] == 'HAMMER'" src="../assets/images/HAMMER.png" class="weapon-img">
                                    <img v-else-if="game['player_weapon'] == 'SCYTHE'" src="../assets/images/SCYTHE.png" class="weapon-img">
                                </div>
                                <div class="form-group col-lg-6">
                                    <label>Enemy weapon</label>
                                    <select class="form-control" required v-model="game['enemy_weapon']">
                                        <option value="SWORD">Sword</option>
                                        <option value="HAMMER">Hammer</option>
                                        <option value="SCYTHE">Scythe</option>
                                    </select>
                                </div>
                                <div class="form-group col-lg-6 text-center">
                                    <img v-if="game['enemy_weapon'] == 'SWORD'" src="../assets/images/enemy_SWORD.png" class="weapon-img">
                                    <img v-else-if="game['enemy_weapon'] == 'HAMMER'" src="../assets/images/enemy_HAMMER.png" class="weapon-img">
                                    <img v-else-if="game['enemy_weapon'] == 'SCYTHE'" src="../assets/images/enemy_SCYTHE.png" class="weapon-img">
                                </div>
                                <div class="form-group col-12">
                                    <label>Minimum grade to win game</label>
                                    <input v-bind:readonly="readOnly" type="number" min="0" class="form-control"
                                        v-model="game['min_win_grade']" required form="task-form">
                                </div>
                                <div class="form-group col-12">
                                    <input v-bind:readonly="readOnly" type="text" class="form-control"
                                        placeholder="Description..." v-model="taskDescription" required form="task-form">
                                </div>
                            </div>

                        </form>
                    </div>
                </div>
                <div class="move-container">
                    <QuizEditorQuestion class="move-block" v-for="(data, index) in questions" :key="data.id" :data="data"
                        :number="index" :remove-self="removeQuestion" :update-self="updateQuestion" :read-only="readOnly"
                        :allowedAnswers="['SINGLE']">
                    </QuizEditorQuestion>
                </div>


                <div class="row m-4" v-if="!readOnly">
                    <div class="col">
                        <div class="plus"><img src="../assets/images/plus.png" style="height: 70px;" @click="addQuestion">
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </section>
</template>