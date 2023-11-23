<script setup>
import { ref } from 'vue';
import QuizTakerQuestion from '../components/taker/QuizTakerQuestion.vue';
import {sendResponse} from "../assets/js/response-api";


const taskTitle = ref("Sample Task"), taskDescription = ref("This is a sample task description."), username = ref("username");

const questions = ref([
    {
        "id":1,
        "type": "SINGLE",
        "description": "What is 2 + 2?",
        "maxGrade": 10,
        "answers": [
            {
                "id": 1,
                "option": "3"
            },
            {
                "id": 2,
                "option": "4"
            },
            {
                "id": 3,
                "option": "5"
            }
        ]
    },
    {
        "id":2,
        "type": "TEXT",
        "description": "What is your name?",
        "maxGrade": 0
    },
    {
        "id":3,
        "type": "TEXT",
        "description": "What is your name?",
        "maxGrade": 0
    },
    {
        "id":4,
        "type": "MULTI",
        "description": "What colors not in rainbow?",
        "maxGrade": 5,
        "answers": [
            {
                "id": 1,
                "option": "Red"
            },
            {
                "id": 2,
                "option": "Black"
            },
            {
                "id": 3,
                "option": "Yellow"
            },
            {
                "id": 4,
                "option": "Gold"
            },
            {
                "id": 5,
                "option": "Gray"
            }
        ]
    }
]);

const answers = {};


const updateAnswer = (questionId, type, value)=>{
    console.log(questionId, type, value);

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


const submitAnswers = ()=>{
    let list = [];

    for(let key in answers){
        list = list.concat(answers[key]);
    }

    const response = {"uid":"", "question_responses":list};
    sendResponse(response);
};

</script>
<template>
    <section class="book_section">
        <div class="container">
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