<script setup>
import { ref, toRaw } from "vue";
import { generateTask } from "../assets/js/task-api";
import LoadingWindow from './LoadingWindow.vue';
import { parseError } from "../assets/js/utilities";

const props = defineProps({
    active: {
        type: Boolean,
        required: true
    },
    close: {
        type: Function,
        required: true
    },
    loadTask: {
        type: Function,
        required: true
    }
});
const errorMessage = ref("");

const closeError = () => {
    errorMessage.value = "";
};

const errorLog = (err) => {
    isActive.value = false;
    console.log(err);

    errorMessage.value = parseError(err);
};

const data = ref({ theme: "", questions: 1, answers: 1 })

const isActive = ref(false);

const submitTask = () => {
    isActive.value = true;

    const request = toRaw(data.value);
    generateTask(request).then(res => {
        props.loadTask(res);
    }).catch(errorLog);
};

</script>

<template>
    <main>
        <LoadingWindow :is-active="isActive"></LoadingWindow>
        <ModalWindow :question="errorMessage" :cancel="closeError"></ModalWindow>

        <div v-bind:class="active ? 'modal-container' : 'modal-container hide'">
            <div class="modal-list" style="max-width: 700px; height: auto;">
                <div class="card" style="margin-top: 20vh;">
                    <div class="card-body">
                        <div class="close"><i class="fa fa-close" @click="close"></i>
                        </div>
                        <form @submit.prevent="submitTask" onsubmit="return false;"
                            style="background: none; box-shadow: none;">
                            <div class="form-row">
                                <div class="form-group col-12">
                                    <h4>Write parameters to generate quiz</h4>
                                </div>

                                <div class="form-group col-12">
                                    <input class="form-control" v-model="data.theme" placeholder="Theme..." type="text"
                                        maxlength="200" minlength="10" required>
                                </div>
                                <div class="form-group col-12 col-md-6">
                                    <label>Questions count</label>
                                    <input class="form-control" v-model="data.questions" type="number" min="1" max="50"
                                        required>
                                </div>
                                <div class="form-group col-12 col-md-6">
                                    <label>Answers in each question count</label>
                                    <input class="form-control" v-model="data.answers" type="number" min="1" max="10"
                                        required>
                                </div>
                            </div>


                            <button type="submit" class="btn btn-primary mt-4" form="task-form">Generate</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </main>
</template>