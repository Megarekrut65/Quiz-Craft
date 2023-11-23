<script setup>
defineProps({
    number: {
        type: Number,
        required: true
    },
    data:{
        type: Object,
        required: true
    }
});

</script>

<template>
    <tr>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-0">{{ number }}</h6>
        </td>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.title }}</h6>
        </td>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.description }}</h6>
        </td>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;"><RouterLink :to="{ name: 'quiz-editor', params: { paramId: data.id } }">Go</RouterLink></h6>
        </td>
    </tr>
</template>