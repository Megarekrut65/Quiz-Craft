<script setup>
import { useRoute } from 'vue-router';
import GameEditor from '../../components/GameEditor.vue';

let {paramId} = useRoute().params;

paramId = paramId?parseInt(paramId):-1;

</script>
<template>
    <GameEditor :param-id="paramId"></GameEditor>
</template>