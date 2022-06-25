import { createStore } from 'vuex'

export default createStore({
    state() {
        return { 
            bestStrategy: {},
            rates: []
        }
    },  

    mutations: {
        setBestStrategy(state, strategy){
            state.bestStrategy = {
                ...strategy,
                buyDate: new Date(strategy.buyDate),
                sellDate: new Date(strategy.sellDate)
            };
        },

        setRates(state, rates){
            const mappedRates = rates.map(rate => ({...rate, date: new Date(rate.date)})) 
            state.rates = mappedRates;
        }
    },

    actions: {
        fetchBestStrategyAndRates({commit}, {startDate, endDate, money}){
            const start = startDate.toISOString().split('T')[0];
            const end = endDate.toISOString().split('T')[0];

            const url = `http://localhost:5000/rates/best?startDate=${start}&endDate=${end}&moneyUsd=${money}`;
            return fetch(url).then(response => {

                if(!response.ok)
                {
                    const message = `An error has occured: ${response.status}`;
                    throw new Error(message)
                }

                return response.json()
            }).then(response => {   
                const strategy = {
                    buyDate: response.buyDate,
                    sellDate: response.sellDate,
                    tool: response.tool,
                    revenue: response. revenue,
                }

                commit('setBestStrategy', strategy)
                commit('setRates', response.rates)
            });
        }
    }
});