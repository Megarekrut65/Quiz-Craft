<script setup>
import { ref, toRaw } from 'vue';
import {saveTask} from "../assets/js/task-api"
import QuizQuestion from '../components/QuizQuestion.vue';

const quizName = ref("Unnamed"), quizDescription = ref("");

const quizzes = ref([]);

const addQuiz = () => {
    quizzes.value.push({ id: quizzes.value.length, description: "", maxGrade: 1.0, type: "SINGLE" });
};

addQuiz();

const shareQuiz = () => {

};

const submitTask = () => {
    const task = {
        title: toRaw(quizName.value),
        description: toRaw(quizDescription.value),
        questions: toRaw(quizzes.value)
    };

    saveTask(task);
};

const updateQuestion = (question) => {
    if (question.id < quizzes.value.length)
        quizzes.value[question.id] = question;
};

const removeQuestion = (id) => {
    console.log(id)
};


</script>
<template>
    <section class="book_section">
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
                                    <input type="text" class="form-control" placeholder="Quiz name" v-model="quizName"
                                        required>

                                </div>
                            </h4>
                            <div class="share">
                                <div>
                                    <button type="button" class="btn btn-success mr-4" @click="submitTask">Save</button>
                                    <button type="button" class="btn btn-warning" @click="shareQuiz">Share</button>
                                </div>
                            </div>

                        </div>

                        <div class="form-row">
                            <div class="form-group col-12">
                                <input type="text" class="form-control" placeholder="Description" v-model="quizDescription">
                            </div>
                        </div>

                    </form>
                </div>
            </div>
            <div class="move-container">
                <QuizQuestion class="move-block" v-for="(data, index) in quizzes" :key="index" :data="data"
                    :remove-self="removeQuestion" :update-self="updateQuestion" >
                </QuizQuestion>
            </div>


            <div class="row m-4">
                <div class="col">
                    <div class="plus"><img src="../assets/images/plus.png" style="height: 70px;" @click="addQuiz"></div>
                </div>
            </div>
        </div>
    </section>
</template>