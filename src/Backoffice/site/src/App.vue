<template>
  <div id="app">
    <div class="row header">
      <div class="col-lg-12">Header</div>
    </div>
    <div class="row">
      <div id="chart-holder" class="col-lg-8">
        <TradingVue msg="Welcome to Your Vue.js App"/>
      </div>
      <div class="col-lg-4">aaa</div>
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
}

#chart-holder {
  margin-top: 50px;
}
</style>
