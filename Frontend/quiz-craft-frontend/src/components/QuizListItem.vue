<script setup>
defineProps({
    number: {
        type: Number,
        required: true
    },
    data:{
        type: Object,
        required: true
    },
    type:{
        type: String,
        required: false,
        default: "Quiz"
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
        <td v-if="type === 'Game'" class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.type }}</h6>
        </td>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">
                <RouterLink v-if="type === 'Quiz'" :to="{ name: 'quiz-editor', params: { paramId: data.id } }">View</RouterLink>
                <RouterLink v-else :to="{ name: 'game-editor', params: { paramId: data.id } }">View</RouterLink>
            </h6>
        </td>
    </tr>
</template>