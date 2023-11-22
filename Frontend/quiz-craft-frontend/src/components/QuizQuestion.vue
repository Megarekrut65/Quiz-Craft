<script setup>
import { ref, toRaw } from 'vue';
import QuizAnswer from './QuizAnswer.vue';
import ModalWindow from './ModalWindow.vue';

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
    description: data.description,
    maxGrade: data.maxGrade,
    type: data.type,
    answers: data.answers
});

props.updateSelf(toRaw(formData));

let id = 0;

const addAnswer = () => {
    formData.value.answers.push({ id: id++, option: "", correct: false });
};

if (data.answers.length == 0) addAnswer();


const updateAnswer = (answer) => {
    const index = formData.value.answers.findIndex(item=>item.id == answer.id);
    if (index != -1)
        formData.value.answers[index] = answer;
};

const removeText = ref(""), removeSubmit = ref(() => { }), removeCancel = ref(() => { });

const closeModal = () => {
    removeText.value = "";

    removeSubmit.value = () => { };
    removeCancel.value = () => { };
};

const removeAnswer = (number, text) => {
    removeText.value = `Do you really want to remove answer #${number + 1} ${text}?`;

    removeSubmit.value = () => {
        formData.value.answers.splice(number, 1);

        if (formData.value.answers.length == 0) addAnswer();

        closeModal();
    };

    removeCancel.value = closeModal;
};

</script>
<template>
    <main>
        <ModalWindow :question="removeText" :submit="removeSubmit" :cancel="removeCancel"></ModalWindow>
        <div class="row mt-4">
            <div class="col">
                <form @change="updateSelf(toRaw(formData))">
                    <div class="form-row ">
                        <div class="close"><i class="fa fa-close" @click="removeSelf(number, formData.description)"></i>
                        </div>
                        <div class="question-number">
                            <h4>{{ number + 1 }}</h4>
                        </div>
                        <div class="form-group col-12 mt-4">
                            <input class="form-control" v-model="formData.description" placeholder="Description"
                                type="text">
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
                    <div class="form-row" style="display: flex; align-items: center; flex-direction: row;">
                        Answers <span class="plus ml-1"><i class="fa fa-plus" @click="addAnswer"></i></span>
                    </div>
                    <div class="form-row mt-2">
                        <div class="form-group col-9">
                            <label>Option</label>
                        </div>
                        <div class="form-group col-3 text-center">
                            <label>Is correct?</label>
                        </div>
                    </div>
                    <QuizAnswer v-for="(data, index) in formData.answers" :key="data.id" :question="formData.id"
                        :data="data" :number="index" :remove-self="removeAnswer" :update-self="updateAnswer"></QuizAnswer>
                </form>
            </div>
        </div>

    </main>
</template>