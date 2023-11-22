<script setup>
import { ref, toRaw } from 'vue';

const props = defineProps({
    data: {
        type: Object,
        required: true
    },
    removeSelf:{
        type: Function,
        required: true
    },
    updateSelf:{
        type: Function,
        required: true
    }
}
);

const data = props.data;

const formData = ref({
    id: data.id,
    description: data.description,
    maxGrade: data.maxGrade,
    type: data.type
});

props.updateSelf(toRaw(formData));

</script>
<template>
    <main>
        <div class="row mt-4">
            <div class="col">
                <form @change="updateSelf(toRaw(formData))">
                    <div class="form-row ">
                        <div class="close"><i class="fa fa-close" @click="removeSelf(data.id)"></i></div>
                        <div class="question-number"><h4>{{ data.id + 1 }}</h4></div>
                        <div class="form-group col-12 mt-4">
                            <input class="form-control" v-model="formData.description" placeholder="Description" type="text">
                        </div>
                        <div class="form-group col-12 col-md-6">
                            <label>Max grade</label>
                            <input class="form-control" v-model="formData.maxGrade" placeholder="Description" type="number">
                        </div>
                        <div class="form-group col-12 col-md-6">
                            <label>Question type</label>
                            <select class="form-control wide" v-model="formData.type">
                                <option value="SINGLE">1 answer</option>
                                <option value="MULTI">Multi answers</option>
                                <option value="TEXT">Full answer</option>
                            </select>
                        </div>               
                    </div>
                </form>
            </div>
        </div>

    </main>
</template>