<script setup>
import { ref, toRaw } from 'vue';


const props = defineProps({
    number: {
        type: Number,
        required: true
    },
    data: {
        type: Object,
        required: true
    },
    updateSelf: {
        type: Function,
        required: true
    }
}
);

const type = props.data.type;

const gradeValue = ref({ "grade": props.data.grade, "id": props.data.responseId });


</script>
<template>
    <main>
        <div class="row mt-4 mb-4">
            <div class="col">
                <form @change="updateSelf(toRaw(gradeValue))">
                    <div class="form-row ">
                        <div class="grade" style="width: 100px;">
                            <div>Max score: {{ data.maxGrade }}</div>
                            <input class="form-control" type="number" min="0" v-bind:max="data.maxGrade" placeholder="Grade"
                                v-model="gradeValue.grade" required form="grade-form">
                        </div>

                        <div class="question-number col-12 mb-4">
                            <h4>{{ number + 1 }}</h4>
                        </div>
                        <div class="form-group col-12 mt-4">
                            <h3>{{ data.description }}</h3>
                        </div>
                    </div>
                    <div v-for="(answer, index) in data.answers" :key="index" class="form-row ">
                        <div class="form-check col-12">
                            <input v-if="type === 'SINGLE'" class="form-check-input" type="radio"
                                v-bind:checked="answer.selected" :value="answer.id" :id="answer.id" disabled>
                            <input v-else-if="type === 'MULTI'" class="form-check-input" type="checkbox"
                                v-bind:checked="answer.selected" :value="answer.id" :id="answer.id" disabled>

                            <label class="form-check-label" v-if="type === 'SINGLE' || type === 'MULTI'" :for="answer.id">
                                {{ answer.option }}
                            </label>
                        </div>
                    </div>
                    <div class="mt-4" v-if="type === 'SINGLE' || type === 'MULTI'">
                        <div style="display: flex; flex-direction: column;">
                            <h6>Correct: </h6>
                            <div v-for="(answer, index) in data.answers" :key="index">
                                <div v-if="answer.correct">
                                    <div> {{ index + 1}}. {{ answer.option }}</div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="form-row" v-if="type === 'TEXT'">
                        <div class="form-group col-12">
                            <input class="form-control" type="text" :value="data.selected" maxlength="300" required
                                placeholder="Answer..." readonly>
                        </div>
                    </div>
                </form>
            </div>
        </div>

    </main>
</template>