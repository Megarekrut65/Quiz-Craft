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
    removeSelf: {
        type: Function,
        required: true
    },
    updateSelf: {
        type: Function,
        required: true
    }
}
);

const data = props.data;

const formData = ref({
    id: data.id,
    option: data.option,
    correct: false
});

props.updateSelf(toRaw(formData));

</script>
<template>
    <main>
        <div class="form-row mt-2" style="position: relative;"  @change="updateSelf(toRaw(formData))">
            <div class="answer-number">
                <div><i class="fa fa-close" @click="removeSelf(number,  data.option)"></i></div>
            </div>
            <div class="form-group col-9">
                <input class="form-control" v-model="formData.option" placeholder="Option" type="text">
            </div>
            <div class="form-group col-3">
                <input class="form-control" style="height: 50%;" type="checkbox" v-model="formData.correct">
            </div>
        </div>
    </main>
</template>