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
            <p class="fw-semibold mb-0">{{ number }}</p>
        </td>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.title }}</p>
        </td>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.description }}</p>
        </td>
        <td v-if="type === 'Game'" class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.type }}</p>
        </td>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">
                <RouterLink v-if="type === 'Quiz'" :to="{ name: 'quiz-editor', params: { paramId: data.id } }">View</RouterLink>
                <RouterLink v-else :to="{ name: 'game-editor', params: { paramId: data.id } }">View</RouterLink>
            </p>
        </td>
    </tr>
</template>