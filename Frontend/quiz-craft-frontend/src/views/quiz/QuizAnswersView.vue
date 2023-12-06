<script setup>
import { ref } from 'vue';

import { getTaskById } from "../../assets/js/task-api";
import { getResponseById, getResponses } from "../../assets/js/response-api";
import { parseError, updateTaskAnswers } from '../../assets/js/utilities';
import { useRoute } from 'vue-router';
import ModalWindow from '../../components/ModalWindow.vue';
import ResponseListItem from "../../components/ResponseListItem.vue";

let { taskId } = useRoute().params;

taskId = parseInt(taskId);

const errorMessage = ref("");

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err) => {
    console.log(err);

    errorMessage.value = parseError(err);
};

const taskTitle = ref(""), taskDescription = ref("");
const questions = ref([]);
let maxGrade = 0;

getTaskById(taskId).then(res => {
    questions.value = res.questions;
    for(let question of res.questions){
        maxGrade+= question["max_grade"];
    }

    taskTitle.value = res.title;
    taskDescription.value = res.description;
}).catch(errorLog);

const responses = ref([]);

getResponses(taskId).then(res => {
    const promises = [];
    for(let i in res){
        const promise = getResponseById(res[i].id).then(resp=>{
            const gradeSum = updateTaskAnswers(questions.value, resp);
            res[i].grade = gradeSum;
            res[i].maxGrade = maxGrade;
        }).catch(errorLog);

        promises.push(promise);
    }

    Promise.all(promises).then(() => {
        responses.value = res;
    }).catch(errorLog);
}).catch(errorLog);

</script>
<template>
    <section class="book_section mb-4">
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
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
                                        required readonly>

                                </div>
                            </h4>
                        </div>

                        <div class="form-row">
                            <div class="form-group col-12">
                                <input readonly type="text" class="form-control" placeholder="Description"
                                    v-model="taskDescription">
                            </div>
                        </div>

                    </form>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="table-responsive mt-4">
                        <table class="table text-nowrap mb-0 align-middle">
                            <thead class="text-dark fs-4">
                                <tr>
                                    <th class="border-bottom-0">
                                        <h6 class="fw-semibold mb-0"><i class="fa fa-hashtag"></i></h6>
                                    </th>
                                    <th class="border-bottom-0">
                                        <h6 class="fw-semibold mb-0">Title</h6>
                                    </th>
                                    <th class="border-bottom-0">
                                        <h6 class="fw-semibold mb-0">Grade</h6>
                                    </th>
                                    <th class="border-bottom-0">
                                        <h6 class="fw-semibold mb-0">Submitting date</h6>
                                    </th>
                                    <th class="border-bottom-0">

                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <ResponseListItem v-for="(response, index) in responses" :key="index" :data="response"
                                    :number="index + 1" :task-id="taskId"></ResponseListItem>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>