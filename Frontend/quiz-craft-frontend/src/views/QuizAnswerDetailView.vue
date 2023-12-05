<script setup>
import { ref } from 'vue';
import ModalWindow from '../components/ModalWindow.vue';
import {getResponseById, gradeResponse} from "../assets/js/response-api";
import { useRoute } from 'vue-router';
import { getTaskById } from '../assets/js/task-api';
import { parseError, updateTaskAnswers } from '../assets/js/utilities';
import QuizGradeQuestion from '../components/grade/QuizGradeQuestion.vue';
import LoadingWindow from "../components/LoadingWindow.vue";

const isActive = ref(true);

let {taskId, responseId} = useRoute().params;

const taskTitle = ref(""), taskDescription = ref(""), username = ref("");
const grade = ref(), maxGrade = ref(0);

const questions = ref([]);

const loaded = ref(false);


const errorMessage = ref("");

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err)=>{
    console.log(err);

    errorMessage.value = parseError(err);
    isActive.value = false;
}

getTaskById(taskId).then(res=>{
    
    taskTitle.value = res.title;
    taskDescription.value = res.description;
    

    questions.value = res.questions.map(item=>{
        const newItem = JSON.parse(JSON.stringify(item));
        newItem.maxGrade = item["max_grade"];
        maxGrade.value += newItem.maxGrade;

        return newItem;
    });


    getResponseById(responseId).then(resp=>{
        username.value = resp.fullname;

        const gradeSum = updateTaskAnswers(questions.value, resp);

        if(gradeSum) grade.value = gradeSum;
        
        isActive.value = false;
    });

    loaded.value = true;
}).catch(errorLog);


const updateGrade = (value)=>{
    const item = questions.value.find((question) => question.responseId === value.id);

    if(item) item.grade = value.grade;
};

const submitGrade = ()=>{
    isActive.value = true;

    const promises = [];

    for (let question of questions.value){
        promises.push(gradeResponse(question.responseId, question.grade));
    }
    Promise.all(promises).then(() => {
        isActive.value = false;
    })
    .catch(errorLog);
};

</script>
<template>
    <section class="book_section">
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
        <LoadingWindow :is-active="isActive"></LoadingWindow>
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
                                    <h5 v-if="grade">{{ grade }}/{{ maxGrade }}</h5>
                                    <p v-else>Max grade: {{ maxGrade }}</p>
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
            <form onsubmit="return false;" @submit="submitGrade" id="grade-form" style="background: none; box-shadow: none; padding: 0; margin: 0;">
                <QuizGradeQuestion v-for="(data, index) in questions" :key="data.responseId" :data="data" :number="index" 
                :update-self="updateGrade">
                </QuizGradeQuestion>
                <div class="row mb-4">
                    <div class="col  text-right">
                        <input type="submit" value="Grade" class="btn btn-success">
                    </div>
                </div>
            </form>
            
        </div>
    </section>
</template>