<script setup>
import { useRoute } from 'vue-router';

let {uid} = useRoute().params;
</script>

<template>
    <section class="book_section">
        <div class="container">
            <div class="row">
                <div class="col">
                    <form>
                        <div>Your answers was submitted! Wait for the teacher to evaluate your response!</div>
                        <RouterLink :to="{ name: 'quiz-taker', params: { uid: uid } }">View your response</RouterLink>
                    </form>
                </div>
            </div>
        </div>
    </section>
</template>