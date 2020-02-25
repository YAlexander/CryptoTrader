<template>
  <trading-vue
    :data="chart"
    :overlays="overlays"
    :width="width"
    :height="570"
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
  data() {
    return {
      chart: Data,
      width: this.width,
      colors: {
        colorBack: "#2b2a2a",
        colorGrid: "#242323",
        colorText: "#eee",
        font: "10px"
      },
      titleTxt: "",
      // Must import all overlays. But they won't displayed while no data
      overlays: [Grin]
    };
  }
};
</script>

<style>
#chart-holder {
  padding-left: 15px;
}
.trading-vue-legend {
  color: yellow;
  background-color: transparent;
  font-size: 8pt;
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
</style>
