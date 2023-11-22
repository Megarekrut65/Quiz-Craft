<script setup>
import { ref, toRaw } from 'vue';
import {saveTask} from "../assets/js/task-api"
import QuizQuestion from '../components/QuizQuestion.vue';
import ModalWindow from '../components/ModalWindow.vue'

const taskTitle = ref("Unnamed"), taskDescription = ref("");

const questions = ref([]);

let id = 0;

const addQuestion = () => {
    questions.value.push({ id: id++, description: "", maxGrade: 1.0, type: "SINGLE" });
};

addQuestion();

const shareTask = () => {

};

const submitTask = () => {
    const task = {
        title: toRaw(taskTitle.value),
        description: toRaw(taskDescription.value),
        questions: toRaw(questions.value)
    };

    saveTask(task);
};

const updateQuestion = (question) => {
    const index = questions.value.findIndex(item=>item.id == question.id);
    if (index != -1)
        questions.value[index] = question;
};

const removeText = ref(""), removeSubmit = ref(()=>{}), removeCancel = ref(()=>{});

const closeModal = ()=>{
    removeText.value = "";

    removeSubmit.value = ()=>{};
    removeCancel.value = ()=>{};
};

const removeQuestion = (number) => {
    removeText.value = `Do you really want to remove question #${number+1}?`;

    removeSubmit.value = ()=>{
        questions.value.splice(number, 1);
        questions.value = toRaw(questions.value);
        if (questions.value.length == 0) addQuestion();

        closeModal();
    };

    removeCancel.value = closeModal;
};


</script>
<template>
    <section class="book_section">
        <ModalWindow :question="removeText" :submit="removeSubmit" :cancel="removeCancel" ></ModalWindow>
        <div class="container">
            <div class="row">
                <div class="col">
                    <form>
                        <div>
                            <h4 class="form-row">
                                <div class="form-group col-lg-2">
                                    <label>Quiz title</label>
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
                <QuizQuestion class="move-block" v-for="(data, index) in questions" :key="data.id" :data="data" :number="index"
                    :remove-self="removeQuestion" :update-self="updateQuestion" >
                </QuizQuestion>
            </div>


            <div class="row m-4">
                <div class="col">
                    <div class="plus"><img src="../assets/images/plus.png" style="height: 70px;" @click="addQuestion"></div>
                </div>
            </div>
        </div>
    </section>
</template>