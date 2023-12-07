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
    },
    type:{
        type:String,
        required: false,
        default: ()=>"Task"
    }
});

</script>

<template>
    <tr>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-0">{{ number }}</p>
        </td>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.fullname }}</p>
        </td>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">{{ data.grade?`${data.grade}/${data.maxGrade}`:"N/A" }}</p>
        </td>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">There will be date</p>
        </td>
        <td class="border-bottom-0">
            <p class="fw-semibold mb-1" style="overflow-x: hidden;">
                <RouterLink v-if="type=='Task'" :to="{ name: 'quiz-answers-detail', params: { taskId: taskId, responseId: data.id } }">
                    Details</RouterLink>
                <RouterLink v-else-if="type=='Game'" :to="{ name: 'game-answers-detail', params: { taskId: taskId, responseId: data.id } }">
                    Details</RouterLink>
            </p>
        </td>
    </tr>
</template>