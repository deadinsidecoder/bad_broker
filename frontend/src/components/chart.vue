<template>
    <highcharts :options="chartOptions"/>
</template>

<script>
import {Chart} from 'highcharts-vue'
import Highcharts from 'highcharts'

Highcharts.theme = {
    colors: ['#63e2b7', '#2a6fff', '#bee8bc', '#78c3fc'],
    chart: {
        backgroundColor: '#303033'
    },
    title: {
        style: {
            color: '#fff',
            font: '16px "Roboto", sans-serif'
        }
    },
    subtitle: {
        style: {
            color: '#696b7a',
            font: '12px "Roboto", sans-serif'
        }
    },
    legend: {
        itemStyle: {
            font: '9pt "Roboto  ", sans-serif',
            color: '#767885'
        },
        itemHoverStyle:{
            color: 'gray'
        }
    },
    xAxis: {
        gridLineColor: '#3a3b42',
    },
    yAxis: {
        gridLineColor: '#3a3b42',
    }
};
Highcharts.setOptions(Highcharts.theme);

export default {

    props:{
        width: {
            type: Number,
            default: 1100
        },
        height: {
            type: Number,
            default: 550
        },
        categories: {
            type: Array
        },
        series: {
            type: Array
        },
        title: {
            type: String
        },
        valueAxisTitle: {
            type: String
        }
    },

    computed: {
        chartOptions(){
            return {
                chart: {
                    type: 'column',
                    width: this.width,
                    height: this.height
                },
                title: {
                    text: this.title
                },
                xAxis: {
                    categories: this.categories,
                    gridLineWidth: 1,
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: this.valueAxisTitle
                    },

                    gridLineWidth: 0
                },
                series: this.series
            };
        }
    },  

    methods: {
        redraw(){
            this.key += 1;
        }
    },

    components: {
        highcharts: Chart
    }
}
</script>

<style>
.chart-wrapper {
    float: left;
    padding-bottom: 40%;
    position: relative;
    width: 50%;
}
</style>