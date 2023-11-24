<script setup>
import {getTaskUid, shareTask, closeTask} from "../assets/js/task-api";
import {copyToBuffer} from "../assets/js/utilities";
import {ref} from "vue";

const props = defineProps({
    active:{
        type: Boolean,
        required: true
    },
    taskId:{
        type:Number,
        required: true
    },
    close: {
        type: Function,
        required: true
    },
    errorLog:{
        type: Function,
        required: true
    }
});

const uidUrl = ref("");

if (props.taskId != -1){
    getTaskUid(props.taskId).then(res=>{
        uidUrl.value = `${window.location.origin}/quiz/${res.uid}`;
    });
}


const share = ()=>{
    shareTask(props.taskId).then(res=>{
        uidUrl.value = `${window.location.origin}/quiz/${res.uid}`;
    }).catch(props.errorLog);
};

const deleteSharing = ()=>{
    closeTask(props.taskId).then(()=>{
        uidUrl.value = "";
    }).catch(props.errorLog);
};

const message = ref("");

const copy = ()=>{
    copyToBuffer(uidUrl.value);
    message.value = "Copied!";

    setTimeout(()=>message.value="", 1000);
};
</script>

<template>
    <main>
        <div v-bind:class="active? 'modal-container' : 'modal-container hide'">
            <div class="modal-list" style="max-width: 500px; width: 100vh; height: auto;">
                <div class="card" style="margin-top: 30vh;">
                    <div class="card-body text-center">
                        <div class="close"><i class="fa fa-close" @click="close"></i>
                        </div>
                        <h5>You can share your quiz to students</h5>
                        <div v-if="uidUrl == ''">

                            <input type="button" class="btn btn-warning py-8 fs-4 mb-1 rounded-2" value="Share"
                                    @click="share">
                        </div>
                        <div v-else>
                            <p>Url to share:</p>
                            <div class="share-url">
                                <input readonly type="text" :value="uidUrl" class="form-control mr-2">
                                <i class="fa fa-copy" style="font-size: 25px;" @click="copy"></i>
                            </div>
                            <div>{{ message }}</div>
                            
                            <input type="button" class="btn btn-danger py-8 fs-4 mb-1 mt-4 rounded-2" value="Close quiz"
                                    @click="deleteSharing">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</template>