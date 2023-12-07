<script setup>
import { ref } from 'vue';

import { getGameById } from "../../assets/js/game-api";
import { getResponseById, getResponses } from "../../assets/js/response-api";
import { parseError, updateTaskAnswers } from '../../assets/js/utilities';
import { useRoute } from 'vue-router';
import ModalWindow from '../../components/ModalWindow.vue';
import ResponseListItem from "../../components/ResponseListItem.vue";
import LoadingWindow from '../../components/LoadingWindow.vue';

let { gameId } = useRoute().params;

gameId = parseInt(gameId);

const errorMessage = ref(""), isActive = ref(true);

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err) => {
    console.log(err);
    isActive.value = false;

    errorMessage.value = parseError(err);
};

const gameTitle = ref(""), gameDescription = ref("");
const questions = ref([]);
let maxGrade = 0;
const responses = ref([]);

getGameById(gameId).then(res => {
    questions.value = res.task.questions;
    for (let question of res.task.questions) {
        maxGrade += question["max_grade"];
    }

    gameTitle.value = res.task.title;
    gameDescription.value = res.task.description;

    getResponses(res.task.id).then(res => {
        const promises = [];
        for (let i in res) {
            const promise = getResponseById(res[i].id).then(resp => {
                const gradeSum = updateTaskAnswers(questions.value, resp);
                res[i].grade = gradeSum;
                res[i].maxGrade = maxGrade;

                return resp;
            }).catch(errorLog);

            promises.push(promise);
        }

        Promise.all(promises).then((result) => {
            responses.value = result;

            isActive.value = false;

        }).catch(errorLog);
    }).catch(errorLog);
}).catch(errorLog);


</script>
<template>
    <section class="book_section mb-4">
        <LoadingWindow :is-active="isActive"></LoadingWindow>
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
        <div class="container">
            <div class="row">
                <div class="col">
                    <form>
                        <div>
                            <h4 class="form-row">
                                <div class="form-group col-lg-2">
                                    <label>Game title</label>
                                </div>
                                <div class="form-group col-lg-6">
                                    <input type="text" class="form-control" placeholder="Quiz name" v-model="gameTitle"
                                        required readonly>

                                </div>
                            </h4>
                        </div>

                        <div class="form-row">
                            <div class="form-group col-12">
                                <input readonly type="text" class="form-control" placeholder="Description"
                                    v-model="gameDescription">
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
                                        <h6 class="fw-semibold mb-0">Fullname</h6>
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
                                    :number="index + 1" :task-id="response.task" type="Game"></ResponseListItem>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>