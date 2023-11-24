import { createRouter, createWebHistory } from 'vue-router'
import { isLoggedAs } from "../assets/js/user-api";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/:pathMatch(.*)*',
      name: '404',
      component: () => import('../views/errors/NotFoundView.vue')
    },{
      path: '/unauthorized',
      name: 'unauthorized',
      component: () => import('../views/errors/UnauthorizedView.vue')
    },
    {
      path: '/',
      name: 'home',
      component: () => import('../views/static/HomeView.vue')
    },
    {
      path: '/quizzes',
      name: 'quizzes',
      component: () => import('../views/QuizzesView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/editor/new',
      name: 'quiz-editor-new',
      component: () => import('../views/QuizEditorNewView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/editor/:paramId',
      name: 'quiz-editor',
      component: () => import('../views/QuizEditorView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/:uid',
      name: 'quiz-taker',
      component: () => import('../views/QuizTakerView.vue')
    },
    {
      path: '/quiz/submitted/:uid',
      name: 'quiz-submitted',
      component: () => import('../views/QuizSubmittedView.vue')
    },
    {
      path: '/quiz/answers/:taskId',
      name: 'quiz-answers',
      component: () => import('../views/QuizAnswersView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/answers/detail/:taskId/:responseId',
      name: 'quiz-answers-detail',
      component: () => import('../views/QuizAnswerDetailView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/about',
      name: 'about',
      component: () => import('../views/static/AboutView.vue')
    },
    {
      path: '/contacts',
      name: 'contacts',
      component: () => import('../views/static/ContactsView.vue')
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue')
    },
    {
      path: '/privacy',
      name: 'privacy',
      component: () => import('../views/static/PrivacyView.vue')
    }
  ]
})

export default router
