<script setup>
import { ref, toRaw } from 'vue';
import { saveTask } from "../assets/js/task-api"
import QuizEditorQuestion from '../components/editor/QuizEditorQuestion.vue';
import ModalWindow from '../components/ModalWindow.vue'

const taskTitle = ref("Unnamed"), taskDescription = ref("");

const questions = ref([]);

let id = 0;

const addQuestion = () => {
    questions.value.push({ id: id++, description: "", maxGrade: 1.0, type: "SINGLE", answers: [] });
};

addQuestion();

const shareTask = () => {

};

const updateQuestion = (question) => {
    const index = questions.value.findIndex(item => item.id == question.id);
    if (index != -1) {
        question.answers = toRaw(question.answers);
        questions.value[index] = question;
    }

};

const removeText = ref(""), removeSubmit = ref(() => { }), removeCancel = ref(() => { });

const closeModal = () => {
    removeText.value = "";

    removeSubmit.value = () => { };
    removeCancel.value = () => { };
};

const removeQuestion = (number, text) => {
    removeText.value = `Do you really want to remove question #${number + 1} ${text}?`;

    removeSubmit.value = () => {
        questions.value.splice(number, 1);

        if (questions.value.length == 0) addQuestion();

        closeModal();
    };

    removeCancel.value = closeModal;
};

const errorMessage = ref("");

const closeError = ()=>{
    errorMessage.value = "";
};

const validateData = (task)=>{
    errorMessage.value = "";

    if(task.title.length == 0){
        errorMessage.value = "Title is empty!";
        return false;
    }
    for(let i in task.questions){
        const question = task.questions[i];

        if(question.description.length == 0){
            errorMessage.value = `Question ${parseInt(i)+1} description is empty!`;
            return false;
        }

        let isCorrect = false;

        for(let j in question.answers){
            const answer = question.answers[j];

            if(answer.option.length == 0){
                errorMessage.value = `Answer ${parseInt(i)+1} option in question '${question.description}'' is empty!`;
                return false;
            }
            isCorrect ||= answer.correct;
        }

        if (!isCorrect){
            errorMessage.value = `There must be at least one correct answer in question '${question.description}''!`;
            return false;
        }
    }

    return true;
}

const extractTask = ()=>{
    const questionsValue = toRaw(questions.value);

    for(let i in questionsValue){
        questionsValue[i].description = questionsValue[i].description.trim();
        if(questionsValue[i].type == "TEXT"){
            questionsValue[i].answers = [];
        }
        for (let j in questionsValue[i].answers){
            questionsValue[i].answers[j].option = questionsValue[i].answers[j].option.trim();
        }
    }

    const task = {
        title: toRaw(taskTitle.value).trim(),
        description: toRaw(taskDescription.value).trim(),
        questions: questionsValue
    };

    return task;
}

const submitTask = () => {
    
    const task = extractTask();
    if(!validateData(task)) return;

    saveTask(task)
    .then(res=>{
        console.log(res);
    })
    .catch(err=>{
        console.log(err);
        errorMessage.value = err.message;
    });
};
</script>
<template>
    <section class="book_section">
        <ModalWindow :question="removeText" :submit="removeSubmit" :cancel="removeCancel"></ModalWindow>
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
        <div class="container">
            <div class="row">
                <div class="col">
                    <form>
                        <div>
                            <h4 class="form-row">
                                <div class="form-group col-lg-2">
                                    <label>Quiz title*</label>
                                </div>
                                <div class="form-group col-lg-6">
                                    <input type="text" class="form-control" placeholder="Quiz name" v-model="taskTitle"
                                        required>

                                </div>
                            </h4>
                            <div class="share">
                                <div>
                                    <button type="button" class="btn btn-success mr-4" @click="submitTask">Save</button>
                                    <button type="button" class="btn btn-warning" @click="shareTask">Share</button>
                                </div>
                            </div>

                        </div>

                        <div class="form-row">
                            <div class="form-group col-12">
                                <input type="text" class="form-control" placeholder="Description" v-model="taskDescription">
                            </div>
                        </div>

                    </form>
                </div>
            </div>
            <div class="move-container">
                <QuizEditorQuestion class="move-block" v-for="(data, index) in questions" :key="data.id" :data="data"
                    :number="index" :remove-self="removeQuestion" :update-self="updateQuestion">
                </QuizEditorQuestion>
            </div>


            <div class="row m-4">
                <div class="col">
                    <div class="plus"><img src="../assets/images/plus.png" style="height: 70px;" @click="addQuestion"></div>
                </div>
            </div>
        </div>
    </section>
</template>