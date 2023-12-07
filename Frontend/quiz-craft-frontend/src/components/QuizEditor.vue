<script setup>
import { ref, toRaw } from 'vue';

import { getTaskById, saveTask } from "../assets/js/task-api"
import QuizEditorQuestion from '../components/editor/QuizEditorQuestion.vue';
import ModalWindow from '../components/ModalWindow.vue'
import ShareWindow from '../components/ShareWindow.vue';
import { parseError } from '../assets/js/utilities';
import { getTaskUid, shareTask, closeTask } from "../assets/js/task-api";
import LoadingWindow from './LoadingWindow.vue';
import GenerateWindow from './GenerateWindow.vue';

const props = defineProps({
    paramId: {
        type: Number,
        required: false,
        default: -1
    }
});

const readOnly = ref(props.paramId != -1 ? true : false);

const taskTitle = ref("Unnamed"), taskDescription = ref("");

const questions = ref([]), maxQuestions = 100;
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
    loaded.value = true;
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

const shareTaskBtn = () => {
    active.value = true;
};

const closeSharing = () => {
    active.value = false;
};

const submitTask = () => {
    const task = extractTask();
    if (!validateData(task)) return;

    modalText.value = "After saving task you can't edit it anymore!";

    modalSubmit.value = () => {
        loaded.value = false;
        
        saveTask(task)
            .then(res => {
                readOnly.value = true;
                window.location = `/quiz/editor/${res.id}`;
            })
            .catch(errorLog);
        closeModal();
    };

    modalCancel.value = closeModal;
};

const generateActive = ref(false);

const loadTaskData = (res) => {
    taskTitle.value = res.title;
    taskDescription.value = res.description;

    questions.value = res.questions.map(item => {
        const newItem = JSON.parse(JSON.stringify(item));
        newItem.maxGrade = item["max_grade"];
        if(!readOnly.value) newItem.id = id++;
        
        return newItem;
    });

    generateActive.value = false;
    loaded.value = true;
};

if (readOnly.value) {
    getTaskById(props.paramId).then(res => {

        loadTaskData(res);
    }).catch(errorLog);
}

const showAnswers = () => {
    window.location = `/quiz/answers/${props.paramId}`;
};


const openGenerate = ()=>{
    generateActive.value = true;
}

const closeGenerate = ()=>{
    generateActive.value = false;
};

</script>
<template>
    <section class="book_section mb-4">
        <ModalWindow :question="modalText" :submit="modalSubmit" :cancel="modalCancel"></ModalWindow>
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
        
        <GenerateWindow :load-task="loadTaskData" :active="generateActive" :close="closeGenerate"></GenerateWindow>

        <ShareWindow :active="active" :obj-id="paramId" :close="closeSharing" :error-log="errorLog" :get-obj="getTaskUid"
            :share-obj="shareTask" :close-obj="closeTask" name="quiz"></ShareWindow>
        
        <LoadingWindow :is-active="!loaded"></LoadingWindow>

        <form id="task-form" @submit.prevent="submitTask" onsubmit="return false;"
            style="background: none; box-shadow: none;">
            <div class="container" v-if="loaded">
                <div class="row">
                    <div class="col">

                        <form onsubmit="return false;">
                            <div>
                                <h4 class="form-row">
                                    <div class="form-group col-lg-2">
                                        <label>Quiz title*</label>
                                    </div>
                                    <div class="form-group col-lg-6">
                                        <input type="text" class="form-control" placeholder="Quiz title..." maxlength="500"
                                            v-model="taskTitle" required v-bind:readonly="readOnly" form="task-form">

                                    </div>
                                </h4>
                                <div class="share">
                                    <div>
                                        <div v-if="!readOnly">
                                            <button type="button" class="btn btn-primary mr-4" form="task-form" @click="openGenerate">BETA
                                                Generate</button>

                                            <button type="submit" class="btn btn-success mr-4"
                                                form="task-form">Save</button>
                                        </div>
                                        <div v-else>
                                            <button type="button" class="btn btn-success mr-3"
                                                @click="showAnswers">Answers</button>
                                            <button type="button" class="btn btn-warning"
                                                @click="shareTaskBtn">Share</button>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="form-row">
                                <div class="form-group col-12">
                                    <input v-bind:readonly="readOnly" type="text" class="form-control" maxlength="1000"
                                        placeholder="Description..." v-model="taskDescription" required form="task-form">
                                </div>
                            </div>

                        </form>
                    </div>
                </div>
                <div class="move-container">
                    <QuizEditorQuestion class="move-block" v-for="(data, index) in questions" :key="data.id" :data="data"
                        :number="index" :remove-self="removeQuestion" :update-self="updateQuestion" :read-only="readOnly">
                    </QuizEditorQuestion>
                </div>


                <div class="row m-4" v-if="!readOnly && questions.length < maxQuestions">
                    <div class="col">
                        <div class="plus"><img src="../assets/images/plus.png" style="height: 70px;" @click="addQuestion">
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </section>
</template>