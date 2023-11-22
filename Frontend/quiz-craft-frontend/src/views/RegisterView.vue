<script setup>
import { RouterLink } from 'vue-router';
import { ref } from 'vue'
import { register } from '../assets/js/register'
import LoadingWindow from '../components/LoadingWindow.vue';

const email = ref(""), password = ref(""), name = ref(""), role = ref("STUDENT");

const error = ref("");
const isLoading = ref(false);

const submit = () => {
    isLoading.value = true;
    register(name.value, email.value, password.value)
        .then(() => isLoading.value = false)
        .catch(err => {
            error.value = err;
            isLoading.value = false;
        });

    return false;
};

</script>

<template>
    <main>
        <div class="position-relative overflow-hidden d-flex align-items-center justify-content-center">
            <div class="d-flex align-items-center justify-content-center">
                <div class="row justify-content-center">
                    <div class="col-12">
                        <div class="card mb-0" style="padding-left: 10px; padding-right: 10px;">
                            <div class="card-body">
                                <div class="text-nowrap logo-img text-center d-block">
                                    <img src="../assets/images/logo.png" height="80" alt="logo">
                                </div>
                                <p class="text-center">Teach and learn</p>
                                <form @submit="submit" action="#" onsubmit="return false;">
                                    <div class="mb-3">
                                        <label for="display-name" class="form-label">Name</label>
                                        <input v-model="name" type="text" class="form-control" name="display-name" required
                                            maxlength="200">
                                    </div>
                                    <div class="mb-4">
                                        <label for="name" class="form-label">Email</label>
                                        <input v-model="email" type="email" class="form-control" name="name" required
                                            minlength="5" maxlength="200">
                                    </div>
                                    <div class="mb-4">
                                        <label for="name" class="form-label">Role</label>
                                        <select v-model="role" class="form-select form-control" name="role" required>
                                            <option value="STUDENT" selected>Student</option>
                                            <option value="TEACHER">Teacher</option>
                                        </select>
                                    </div>
                                    <div class="mb-4">
                                        <label for="password" class="form-label">Password</label>
                                        <input v-model="password" type="password" class="form-control" name="password"
                                            required minlength="8" maxlength="1000">
                                    </div>
                                    <div class="d-flex align-items-center justify-content-between mb-4">
                                        <p class="fs-4 mb-0 fw-bold">Already have account?</p>
                                        <RouterLink class="text-primary fw-bold ms-2" to="/login">
                                            Sing in
                                        </RouterLink>
                                    </div>
                                    <div class="d-flex align-items-center mb-4">
                                        <p class="fs-4 mb-0 fw-bold">
                                            By clicking 'Sign up', you agree to our privacy
                                            <a href="/privacy" target="_blank">policy</a>
                                        </p>
                                    </div>

                                    <div style="display: flex; justify-content: space-between;">
                                        <input type="submit" class="btn btn-primary py-8 fs-4 rounded-2" value="Sign up">
                                    </div>
                                    <div class="d-flex align-items-center mt-4">
                                        <p class="fs-4 mb-0 text-danger">{{ error }}</p>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <LoadingWindow :is-active="isLoading"></LoadingWindow>
    </main>
</template>