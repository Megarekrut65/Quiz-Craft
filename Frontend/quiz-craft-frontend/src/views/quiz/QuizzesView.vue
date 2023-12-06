<script setup>
import { getTasks } from '../../assets/js/task-api';
import { ref } from "vue";
import QuizListItem from '../../components/QuizListItem.vue';
import ModalWindow from '../../components/ModalWindow.vue';
import { parseError } from "../../assets/js/utilities";
import LoadingWindow from '../../components/LoadingWindow.vue';

const errorMessage = ref(""), isActive = ref(true);

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err) => {
    console.log(err);
    isActive.value = false;

    errorMessage.value = parseError(err);
}

const tasks = ref([]);

getTasks().then(res => {
    tasks.value = res;
    isActive.value = false;
}).catch(errorLog);

</script>

<template>
    <main>
        <LoadingWindow :is-active="isActive"></LoadingWindow>
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
        <div class="container mb-4">
            <div class="row">
                <div class="col-12" v-if="tasks.length > 0">
                    <h2>Your quizzes<RouterLink to="/quiz/editor/new"><i class="fa fa-plus"></i></RouterLink></h2>
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
                                        <h6 class="fw-semibold mb-0">Description</h6>
                                    </th>
                                    <th class="border-bottom-0">

                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <QuizListItem v-for="(task, index) in tasks" :key="index" :data="task" :number="index + 1">
                                </QuizListItem>
                            </tbody>
                        </table>
                    </div>

                </div>
                <div class="col-12" v-else>
                    <h2>Your quizzes</h2>
                    <h5>There no quizzes yet! Create one? <RouterLink to="quiz/editor/new">Create</RouterLink>
                    </h5>
                </div>
            </div>

        </div>

    </main>
</template>