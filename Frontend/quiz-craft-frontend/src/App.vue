<script setup>
import { ref } from "vue";
import { subscribeUserChangeState, userIsLogged, getUsername, logout } from "./assets/js/user-api";

const isLogged = ref(userIsLogged()), username = ref(getUsername());

const changeState = (logged) => {
  isLogged.value = logged;
  username.value = getUsername();
};

const logoutUser = () => {
  logout().then(() => {
    window.location = "/login";
  }).catch(err => {
    console.log(err);
  });
}

subscribeUserChangeState(changeState);
</script>

<template>
  <header class="header_section" style="position: fixed; width: 100vw; z-index: 10000;">
    <div class="header_bottom">
      <div class="container-fluid">
        <nav class="navbar navbar-expand-lg custom_nav-container ">
          <RouterLink class="navbar-brand" to="/" style="padding: 0;">
            <img src="./assets/images/logo.png" alt="Logo" style="width: 40px;">
          </RouterLink>


          <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class=""> </span>
          </button>

          <div class="collapse navbar-collapse" id="navbarSupportedContent">
            <div class="d-flex mr-auto flex-column flex-lg-row align-items-center">
              <ul class="navbar-nav  ">
                <li class="nav-item active">
                  <RouterLink class="nav-link" to="/">Home</RouterLink>
                </li>
                <li class="nav-item" v-if="isLogged">
                  <div class="dropdown">
                    <button class="btn dropdown-toggle nav-link" type="button" id="dropdownMenuButton"
                      data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                      Content
                    </button>
                    <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                      <RouterLink class="dropdown-item" to="/quizzes">My Quizzes</RouterLink>
                      <RouterLink class="dropdown-item" to="/games">My Games</RouterLink>
                    </div>
                  </div>
                  
                </li>
                <li class="nav-item" v-if="isLogged">
                  <div class="dropdown">
                    <button class="btn dropdown-toggle nav-link" type="button" id="dropdownMenuButton"
                      data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                      Create
                    </button>
                    <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                      <RouterLink class="dropdown-item" to="/quiz/editor/new">Quiz</RouterLink>
                      <RouterLink class="dropdown-item" to="/game/editor/new">Game</RouterLink>
                    </div>
                  </div>
                  
                </li>
                <li class="nav-item">
                  <RouterLink class="nav-link" to="/about">About</RouterLink>
                </li>
                <li class="nav-item">
                  <RouterLink class="nav-link" to="/contacts">Contact Us</RouterLink>
                </li>
                <li class="nav-item">
                  <RouterLink class="nav-link" to="/privacy">Privacy policy</RouterLink>
                </li>
              </ul>
            </div>
            <div class="user-header" v-if="isLogged">
              <div class="mr-4">{{ username }}</div>
              <div @click="logoutUser" class="logout nav-link">Logout</div>
            </div>
            <div class="quote_btn-container" v-else>
              <RouterLink to="/login">
                <i class="fa fa-user" aria-hidden="true"></i>
                <span>
                  Sign In
                </span>
              </RouterLink>
              <RouterLink to="/register">
                <i class="fa fa-user" aria-hidden="true"></i>
                <span>
                  Sign Up
                </span>
              </RouterLink>

            </div>
          </div>
        </nav>
      </div>
    </div>
  </header>


  <div class="container-fluid" style="min-height: 100vh; padding-top: 80px;">
    <RouterView />
  </div>


  <section class="info_section mt-4">
    <div class="container">
      <div class="info_bottom layout_padding2">
        <div class="row info_main_row">
          <div class="col-md-6 col-lg-3 info_top">
            <div class="info_logo">
              <a href="">
                <img src="./assets/images/logo.png" alt="">
              </a>
            </div>
          </div>
          <div class="col-md-6 col-lg-3">
            <h5>
              Address
            </h5>
            <div class="info_contact">
              <a href="">
                <i class="fa fa-envelope"></i>
                <span>
                  example@gmail.com
                </span>
              </a>
            </div>
            <div class="social_box">
              <a href="">
                <i class="fa fa-facebook" aria-hidden="true"></i>
              </a>
              <a href="">
                <i class="fa fa-twitter" aria-hidden="true"></i>
              </a>
              <a href="">
                <i class="fa fa-linkedin" aria-hidden="true"></i>
              </a>
              <a href="">
                <i class="fa fa-instagram" aria-hidden="true"></i>
              </a>
            </div>
          </div>
          <div class="col-md-6 col-lg-3">
            <div class="info_links">
              <h5>
                Useful link
              </h5>
              <div class="info_links_menu">
                <RouterLink to="/">
                  Home
                </RouterLink>
                <RouterLink to="/about">
                  About
                </RouterLink>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

