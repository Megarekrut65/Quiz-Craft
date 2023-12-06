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
      component: () => import('../views/quiz/QuizzesView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/games',
      name: 'games',
      component: () => import('../views/game/GamesView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/editor/new',
      name: 'quiz-editor-new',
      component: () => import('../views/quiz/QuizEditorNewView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/game/editor/new',
      name: 'game-editor-new',
      component: () => import('../views/game/GameEditorNewView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/game/editor/:paramId',
      name: 'game-editor',
      component: () => import('../views/game/GameEditorView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/editor/:paramId',
      name: 'quiz-editor',
      component: () => import('../views/quiz/QuizEditorView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/:uid',
      name: 'quiz-taker',
      component: () => import('../views/quiz/QuizTakerView.vue')
    },
    {
      path: '/quiz/submitted/:uid',
      name: 'quiz-submitted',
      component: () => import('../views/quiz/QuizSubmittedView.vue')
    },
    {
      path: '/quiz/answers/:taskId',
      name: 'quiz-answers',
      component: () => import('../views/quiz/QuizAnswersView.vue'),
      beforeEnter: (to, from, next) => isLoggedAs(to, from, next, "TEACHER")
    },
    {
      path: '/quiz/answers/detail/:taskId/:responseId',
      name: 'quiz-answers-detail',
      component: () => import('../views/quiz/QuizAnswerDetailView.vue'),
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
