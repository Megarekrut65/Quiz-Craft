import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { 
      path: '/:pathMatch(.*)*', 
      name: '404',
      component: () => import('../views/NotFoundView.vue')
    },
    {
      path: '/',
      name: 'home',
      component: () => import('../views/HomeView.vue')
    }
    ,
    {
      path: '/quizzes',
      name: 'quizzes',
      component: () => import('../views/QuizzesView.vue')
    },
    {
      path: '/quiz/editor/new',
      name: 'quiz-editor-new',
      component: () => import('../views/QuizEditorNewView.vue')
    },
    {
      path: '/quiz/editor/:paramId',
      name: 'quiz-editor',
      component: () => import('../views/QuizEditorView.vue')
    },
    {
      path: '/quiz/:uid',
      name: 'quiz-taker',
      component: () => import('../views/QuizTakerView.vue')
    },
    {
      path: '/quiz/submitted',
      name: 'quiz-submitted',
      component: () => import('../views/QuizSubmittedView.vue')
    },
    {
      path: '/about',
      name: 'about',
      component: () => import('../views/AboutView.vue')
    },
    {
      path: '/contacts',
      name: 'contacts',
      component: () => import('../views/ContactsView.vue')
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
      component: () => import('../views/PrivacyView.vue')
    }
  ]
})

export default router
