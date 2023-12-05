<script setup>
import QuizEditor from './QuizEditor.vue';

defineProps({
    paramId: {
        type: Number,
        required: false,
        default: -1
    }
});
</script>
<template>
    <section class="book_section mb-4">
        <div class="container">
            <div class="row">
                <div class="col">

                    <form onsubmit="return false;">
                        <div>
                            <h5 class="form-row">
                                <div class="form-group col-lg-2">
                                    <h5>Game type</h5>
                                </div>
                                <div class="form-group col-lg-6">
                                    <select class="form-control" required  value="Single answer game">
                                        <option option="SINGLEANSWER">Single answer game</option>
                                    </select>

                                </div>
                            </h5>
                            <div class="share">
                                <div>
                                    <button v-if="!readOnly" type="submit" class="btn btn-success mr-4"
                                        form="task-form">Save</button>
                                    <div v-else>
                                        <button type="button" class="btn btn-success mr-3"
                                            @click="showAnswers">Answers</button>
                                        <button type="button" class="btn btn-warning" @click="shareTask">Share</button>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="form-row">
                            <div class="form-group col-12">
                                <input v-bind:readonly="readOnly" type="text" class="form-control"
                                    placeholder="Description..." v-model="taskDescription" required form="task-form">
                            </div>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    </section>
    <QuizEditor :param-id="paramId"></QuizEditor>
</template>