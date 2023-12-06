<script setup>
import { useRoute } from 'vue-router';
import QuizEditor from '../../components/QuizEditor.vue';

let {paramId} = useRoute().params;

paramId = paramId?parseInt(paramId):-1;

</script>
<template>
    <QuizEditor :param-id="paramId"></QuizEditor>
</template>