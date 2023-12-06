<script setup>
defineProps({
    number: {
        type: Number,
        required: true
    },
    data: {
        type: Object,
        required: true
    },
    taskId:{
        type:Number,
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
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.fullname }}</h6>
        </td>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.grade?`${data.grade}/${data.maxGrade}`:"N/A" }}</h6>
        </td>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">There will be date</h6>
        </td>
        <td class="border-bottom-0">
            <h6 class="fw-semibold mb-1" style="overflow-x: hidden;">
                <RouterLink :to="{ name: 'quiz-answers-detail', params: { taskId: taskId, responseId: data.id } }">
                    Details</RouterLink>
            </h6>
        </td>
    </tr>
</template>