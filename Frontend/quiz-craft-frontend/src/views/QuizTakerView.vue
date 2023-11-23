<script setup>
import { ref } from 'vue';
import QuizTakerQuestion from '../components/taker/QuizTakerQuestion.vue';
import ModalWindow from '../components/ModalWindow.vue';
import {sendResponse} from "../assets/js/response-api";
import { useRoute } from 'vue-router';
import { getUsername } from '../assets/js/user-api';
import { getTaskByUid } from '../assets/js/task-api';
import { parseError } from '../assets/js/utilities';

let {uid} = useRoute().params;

const taskTitle = ref(""), taskDescription = ref(""), username = ref(getUsername());

const questions = ref([]);

const answers = {};
const loaded = ref(false);


const updateAnswer = (questionId, type, value)=>{

    if (type == "TEXT"){
        answers[questionId] = [{"question_id": questionId, "text_answer": value}];
        return;
    }

    if(type == "MULTI"){
        answers[questionId] = value.map(item=>{return {"question_id": questionId, "answer_id": item}});
        return;
    }

    answers[questionId] = [{"question_id": questionId, "answer_id": value}];
};


const errorMessage = ref("");

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err)=>{
    console.log(err);

    errorMessage.value = parseError(err);
}

const submitAnswers = ()=>{
    let list = [];

    for(let key in answers){
        list = list.concat(answers[key]);
    }

    const response = {"uid":uid, "question_responses":list};

    sendResponse(response).then(()=>{
        window.location = "/quiz/submitted";
    }).catch(errorLog)
};


getTaskByUid(uid).then(res=>{
    taskTitle.value = res.title;
    taskDescription.value = res.description;

    questions.value = res.questions.map(item=>{
        const newItem = JSON.parse(JSON.stringify(item));
        newItem.maxGrade = item["max_grade"];
        return newItem;
    });

    loaded.value = true;
}).catch(errorLog)

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
                                    <p>You are signed as {{ username }}</p>
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
            <QuizTakerQuestion v-for="(data, index) in questions" :key="index" :data="data" :number="index" :update-self="updateAnswer">
            </QuizTakerQuestion>

            <div class="row mb-4">
                <div class="col text-right">
                    <input type="button" value="Submit" class="btn btn-success" @click="submitAnswers">
                </div>
            </div>
        </div>
    </section>
</template>