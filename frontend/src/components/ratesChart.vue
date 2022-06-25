<template>
    <div>
        <chart :categories="categories" 
            :series="series" 
            title="Rates" 
            valueAxisTitle="USD / Currency"
            v-show="series.length"/>
    </div>
</template>

<script>
import chart from './chart'

export default {

    computed: {
        categories(){
            return this.$store.state.rates.map(x => x.date.toISOString().split('T')[0]);
        },

        series(){
            const arrayOfSeries = [];
            const currencies = Object.keys(this.$store.state.rates[0] ?? {}).filter(x => x != 'date');
            currencies.forEach(currency => {
                const series = {
                    name: currency.toUpperCase(),
                    type: 'line',
                    data: this.$store.state.rates.map(x => x[currency])
                };

                arrayOfSeries.push(series);
            });

            return arrayOfSeries;
        }
    },

    components: {
        chart
    }
}
</script>

<style>

</style>