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
    updateSelf:{
        type: Function,
        required: true
    }
}
);

const type = props.data.type;

const answerValue = (type == "MULTI")?ref([]):ref();

if (type == "SINGLE") {
    answerValue.value = props.data.answers[0].id;
}


</script>
<template>
    <main>
        <div class="row mt-4 mb-4">
            <div class="col">
                <form @change="updateSelf(data.id, type, toRaw(answerValue))">
                    <div class="form-row ">
                        <div class="grade">Score: {{ data.maxGrade }}</div>
                        <div class="question-number">
                            <h4>{{ number + 1 }}</h4>
                        </div>
                        <div class="form-group col-12 mt-4">
                            <h3>{{ data.description }}</h3>
                        </div>
                    </div>
                    <div v-for="(answer, index) in data.answers" :key="index" class="form-row ">
                        <div class="form-check col-12">
                            <input v-if="type == 'SINGLE'" class="form-check-input" type="radio" v-model="answerValue"
                                :value="answer.id" :id="answer.id">
                            <input v-else-if="type == 'MULTI'" class="form-check-input" type="checkbox"
                                v-model="answerValue" :value="answer.id" :id="answer.id">

                            <label class="form-check-label" v-if="type == 'SINGLE' || type == 'MULTI'" :for="answer.id">
                                {{ answer.option }}
                            </label>
                        </div>
                    </div>
                    <div class="form-row" v-if="type == 'TEXT'">
                        <div class="form-group col-12">
                            <input class="form-control" type="text" v-model="answerValue"
                                maxlength="300" required placeholder="Answer...">
                        </div>
                    </div>
                </form>
            </div>
        </div>

    </main>
</template>