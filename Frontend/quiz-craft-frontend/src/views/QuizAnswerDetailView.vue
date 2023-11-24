<script setup>
import { ref } from 'vue';
import ModalWindow from '../components/ModalWindow.vue';
import {getResponseById} from "../assets/js/response-api";
import { useRoute } from 'vue-router';
import { getTaskById } from '../assets/js/task-api';
import { parseError, updateTaskAnswers } from '../assets/js/utilities';
import QuizTakenQuestion from '../components/taker/QuizTakenQuestion.vue';

let {taskId, responseId} = useRoute().params;

const taskTitle = ref(""), taskDescription = ref(""), username = ref("");

const questions = ref([]);

const loaded = ref(false);


const errorMessage = ref("");

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err)=>{
    console.log(err);

    errorMessage.value = parseError(err);
}

getTaskById(taskId).then(res=>{
    console.log(res);

    taskTitle.value = res.title;
    taskDescription.value = res.description;
    username.value = res.fullname;

    questions.value = res.questions.map(item=>{
        const newItem = JSON.parse(JSON.stringify(item));
        newItem.maxGrade = item["max_grade"];
        return newItem;
    });


    getResponseById(responseId).then(resp=>{
        console.log(resp);
        
        updateTaskAnswers(questions.value, resp);
        console.log(questions)
    });

    loaded.value = true;
}).catch(errorLog);


</script>
<template>
    <section class="book_section">
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
        <div class="container" v-if="loaded">
            <div class="row">
                <div class="col">
                    <form>
                        <div>
                            <div class="form-row">
                                <div class="form-group col-12 col-md-6">
                                    <h2>{{ taskTitle }}</h2>
                                </div>
                                <div class="form-group col-12 col-md-6 text-md-right text-left">
                                    <p>Submitted by {{ username }}</p>
                                </div>
                            </div>
                        </div>

                        <div class="form-row">
                            <div class="form-group col-12">
                                <p>{{ taskDescription }}</p>
                            </div>
                        </div>

                    </form>
                </div>
            </div>
            <QuizTakenQuestion v-for="(data, index) in questions" :key="index" :data="data" :number="index">
            </QuizTakenQuestion>
            
        </div>
    </section>
</template>