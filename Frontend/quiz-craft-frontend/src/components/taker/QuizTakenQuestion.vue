<script setup>


defineProps({
    number: {
        type: Number,
        required: true
    },
    data: {
        type: Object,
        required: true
    }
}
);


</script>
<template>
    <main>
        <div class="row mt-4 mb-4">
            <div class="col">
                <form>
                    <div class="form-row ">
                        <div class="grade" v-if="data.grade != null && data.grade != undefined">Score: {{ data.grade }}/{{ data.maxGrade }}</div>
                        <div class="grade" v-else>Max score: {{ data.maxGrade }}</div>

                        <div class="question-number">
                            <h4>{{ number + 1 }}</h4>
                        </div>
                        <div class="form-group col-12 mt-4">
                            <h3>{{ data.description }}</h3>
                        </div>
                    </div>
                    <div v-for="(answer, index) in data.answers" :key="index" class="form-row ">
                        <div class="form-check col-12">
                            <input v-if="data.type === 'SINGLE'" class="form-check-input" type="radio" v-bind:checked="answer.selected"
                                :value="answer.id" :id="answer.id" disabled>

                            <input v-else-if="data.type === 'MULTI'" class="form-check-input" type="checkbox"
                                v-bind:checked="answer.selected" :value="answer.id" :id="answer.id" disabled>

                            <label class="form-check-label" v-if="data.type === 'SINGLE' || data.type === 'MULTI'" :for="answer.id">
                                {{ answer.option }}
                            </label>
                        </div>
                    </div>
                    <div class="form-row" v-if="data.type === 'TEXT'">
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