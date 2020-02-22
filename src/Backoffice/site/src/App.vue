<template>
  <div id="app">
    <div class="row">
      <div id="chart-holder" class="col-lg-6">
        <TradingVue msg="Welcome to Your Vue.js App"/>
      </div>
      <div class="col-lg-6">aaa</div>
    </div>
  </div>
</template>

<script>
import TradingVue from "./components/TradingView.vue";

export default {
  name: "App",
  components: {
    TradingVue
  }
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
