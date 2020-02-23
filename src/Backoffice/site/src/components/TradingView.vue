<style>
.trading-vue-legend {
  color: yellow;
  background-color: transparent;
  top: 0px;
}

.trading-vue-legend span {
  background-color: transparent !important;
}

.t-vue-ind,
.trading-vue-ohlcv {
  margin: 0;
  padding: 0;
}

.trading-vue-chart {
  top: 0px;
}

.trading-vue-legend {
  visibility: hidden;
}
</style>

<template>
  <trading-vue
    :data="chart"
    :overlays="overlays"
    :width="width"
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
    var elem = document.getElementById("chart-holder");
    this.width = elem.offsetWidth;
  },
  beforeDestroy() {
    window.removeEventListener("resize", this.onResize);
  },
  width: 0,
  data() {
    return {
      chart: Data,
      width: this.width,
      colors: {
        colorBack: "#fff",
        colorGrid: "#242323",
        colorText: "#eee"
      },
      titleTxt: "",
      // Must import all overlays. But they won't displayed while no data
      overlays: [Grin]
    };
  }
};
</script>
