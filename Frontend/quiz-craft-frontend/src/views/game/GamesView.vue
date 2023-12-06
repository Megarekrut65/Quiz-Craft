<script setup>
import { getGames } from '../../assets/js/game-api';
import { ref } from "vue";
import QuizListItem from '../../components/QuizListItem.vue';
import ModalWindow from '../../components/ModalWindow.vue';
import {parseError} from "../../assets/js/utilities";

const errorMessage = ref("");

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err)=>{
    console.log(err);

    errorMessage.value = parseError(err);
}

const games=ref([]);

getGames().then(res => {
    games.value = res.map(game=>{
        return {id:game.id, title: game.task.title, description: game.task.description, type: game.type};
    });
}).catch(errorLog);

</script>

<template>
    <main>
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>
        <div class="container mb-4">
            <div class="row">
                <div class="col-12 mt-4" v-if="games.length > 0">
                    <h2>Your games <RouterLink to="/game/editor/new"><i class="fa fa-plus"></i></RouterLink></h2>
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
                                        <h6 class="fw-semibold mb-0">Type</h6>
                                    </th>
                                    <th class="border-bottom-0">
                                        
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <QuizListItem v-for="(game, index) in games" :key="index" :data="game" :number="index+1" type="Game"></QuizListItem>
                            </tbody>
                        </table>
                    </div>

                </div>
                <div class="col-12" v-else>
                    <h2>Your games</h2>
                    <h5>There no games yet! Create one? <RouterLink to="game/editor/new">Create</RouterLink></h5>
                </div>
            </div>
        </div>

    </main>
</template>