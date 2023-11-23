<script setup>
import { getTasks } from '../assets/js/task-api';
import { ref } from "vue";
import QuizListItem from '../components/QuizListItem.vue';

const tasks = ref([{ "title": "hello", "description": "dwwwd" , "id":12}]);

getTasks().then(res => {
    tasks.value = res;
    console.log(res);
}).catch(err => {
    console.log(err);
})
</script>

<template>
    <main>
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <h2>Your quizzes</h2>
                    <div class="table-responsive mt-4"
                        @click="(event) => event.target.classList.contains('ti-x') ? () => { } : remove()">
                        <table class="table text-nowrap mb-0 align-middle">
                            <thead class="text-dark fs-4">
                                <tr>
                                    <th class="border-bottom-0">
                                        <h6 class="fw-semibold mb-0"><i class="ti ti-list-numbers"></i></h6>
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
                                <QuizListItem v-for="(task, index) in tasks" :key="index" :data="task" :number="index+1"></QuizListItem>
                            </tbody>
                        </table>
                    </div>

                </div>
            </div>
        </div>

    </main>
</template>