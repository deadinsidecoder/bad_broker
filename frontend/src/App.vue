<template>
    <n-config-provider :theme="darkTheme">
        <div class="logo">
            <h1>Bad broker</h1>
        </div>
        <Form/>
        <rates-chart class="chart"/>
        <best-strategy class="best-strategy"/>
    </n-config-provider>
</template>

<script>
import { NConfigProvider, darkTheme } from 'naive-ui'
import Form from './components/form'
import RatesChart from './components/ratesChart'
import BestStrategy from './components/bestStrategy'

export default {
    name: 'App',  

    setup(){
        return {
            darkTheme
        }
    },

    components: {
        NConfigProvider,
        Form,
        RatesChart,
        BestStrategy
    }
}
</script>

<style lang="scss">
@import './assets/scss/style.scss';

.logo{
    margin-top: 60px;
    margin-bottom: 25px;
    text-align: center;
} 

.chart{
    margin-top: 40px;
    display:flex;
    justify-content: center;
}

.best-strategy{
    display:flex;
    justify-content: center;
    margin-top: 20px;
}
</style>
