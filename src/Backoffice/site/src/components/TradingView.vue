<style scoped>
</style>

<template>
  <trading-vue
    :data="chart"
    :overlays="overlays"
    :width="this.width"
    :color-back="colors.colorBack"
    :color-grid="colors.colorGrid"
    :color-text="colors.colorText"
    :titleTxt="titleTxt"
  ></trading-vue>
</template>

<script>
import TradingVue from "trading-vue-js";
import Data from "../../data/data.json";
import Grin from "../indicators/Grin.js";

export default {
  name: "app",
  components: { TradingVue },
  methods: {
    onResize() {
      var elem = document.getElementById("chart-holder");
      this.width = elem.offsetWidth;
    }
  },
  mounted() {
    window.addEventListener("resize", this.onResize);
  },
  beforeDestroy() {
    window.removeEventListener("resize", this.onResize);
  },
  data() {
    return {
      chart: Data,
      width: this.width,
      colors: {
        colorBack: "#2b2a2a",
        colorGrid: "#242323",
        colorText: "#eee"
      },
      titleTxt: "",
      overlays: [Grin]
    };
  }
};
</script>
