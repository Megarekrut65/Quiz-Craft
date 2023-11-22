<script setup>

defineProps({
    question:{
        type: String,
        required: true
    },
    submit: {
        type: Function,
        required: true
    },
    cancel: {
        type: Function,
        required: true
    }
});


</script>

<template>
    <main>
        <div v-bind:class="question !== ''? 'modal-container' : 'modal-container hide'" @click="cancel">
            <div class="modal-list" style="max-width: 400px; width: 100vh; height: auto;">
                <div class="card" style="margin-top: 30vh;">
                    <div class="card-body text-center">
                        <h4>{{ question }}</h4>
                        

                        <div style="display: flex; justify-content: space-around;" class="mt-4">
                            <input type="button" class="btn btn-success py-8 fs-4 mb-1 rounded-2" value="Ok"
                                @click="submit">
                            <input type="button" class="btn btn-danger py-8 fs-4 mb-1 rounded-2" value="Cancel"
                                @click="cancel">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</template>