<script setup>

const props = defineProps({
    question:{
        type: String,
        required: true
    },
    submit: {
        type: Function,
        required: false
    },
    cancel: {
        type: Function,
        required: true
    },
    z_index:{
        type:Number,
        required: false,
        default: ()=>9999
    }
});

const close = (event)=>{
    if(event.target.className == "modal-container") props.cancel();
};

</script>

<template>
    <main>
        <div v-bind:class="question !== ''? 'modal-container' : 'modal-container hide'" @click="close" :style="`z-index: ${z_index};`">
            <div class="modal-list" style="max-width: 500px; width: 100vh; height: auto;">
                <div class="card" style="margin-top: 30vh;">
                    <div class="card-body text-center">
                        <h5>{{ question }}</h5>
                        

                        <div style="display: flex; justify-content: space-around;" class="mt-4">
                            <input v-if="submit" type="button" class="btn btn-success py-8 fs-4 mb-1 rounded-2" value="Ok"
                                @click="submit">
                            <input type="button" class="btn btn-danger py-8 fs-4 mb-1 rounded-2" :value="submit?'Cancel':'Ok'"
                                @click="cancel">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</template>