<template>
    <div class="center">
        <nav class="navbar navbar-expand-sm bg-dark navbar-dark fixed-top">
            <div class="container-fluid">
                <a class="navbar-brand" href="https://localhost:5002/">Not A Lottery</a>
                <button v-if="accountNum === null" @click="ConnectMetaMask()" type="button" class="btn btn-dark connect-btn">Connect MetaMask</button>
                <button v-else type="button" class="btn btn-dark connected-btn">{{ accountNum }}</button>
            </div>
        </nav>
        
        <div class="page-header">
            <h1>Welcome!</h1>
            <p>When in doubt, refresh the page</p>
            <br />
            <p v-if="winningsTotalPulse !== null && winningsTotalPulse > 0">You've won {{ winningsTotalPulse }} of PLS!!! Congrats!!!</p>
            <div><button type="button" class="btn btn-success" @click="ClaimWinnings()" :disabled="winningsTotalPulse === null || winningsTotalPulse === 0">Claim All Winnings!!!</button></div>
            <br />
        </div>

        <div class="row row-cols-1 row-cols-md-3 g-4" style="padding: 5% 10%;" v-if="accountNum !== null && lottoTypes !== null && lottoTimes !== null && ticketsBought !== null">
            <Lotto  
                v-for="(t, index) in lottoTypes" :key="t"
                :addressNumber="accountNum" 
                :lottoType="lottoTypes[index]" 
                :timeEnd="lottoTimes[index].dateAndTime"
                :ticketsBoughtIncoming="ticketsBought[index]"
                @refreshLotto="RefreshLotto()"
            />
        </div>
        <div class="row row-cols-1 row-cols-md-3 g-4" style="padding: 5% 10%;" v-if="accountNum === null && lottoTypes !== null && lottoTimes !== null && ticketsBought === null">
            <Lotto  
                v-for="(t, index) in lottoTypes" :key="t"
                :addressNumber="null" 
                :lottoType="lottoTypes[index]" 
                :timeEnd="lottoTimes[index].dateAndTime"
                :ticketsBoughtIncoming="0"
            />
        </div>
        
    </div>
</template>

<script>
    import MetaMaskSDK from '@metamask/sdk';
    import Lotto from './components/Lotto_Card.vue';
    // import detectEthereumProvider from '@metamask/detect-provider';// This function detects most providers injected at window.ethereum.
    const MMSDK = new MetaMaskSDK();
    const ethereum = await MMSDK.getProvider(); // You can also access via window.ethereum
    // const provider = await detectEthereumProvider();// This returns the provider, or null if it wasn't detected.
    // const BN = require('bn.js');
    
export default {
  name: 'App',
  data() {
        return {
            accountNum: null,
            eth: null,
            connected: null,
            lottoTimes: null,
            winnerLists: null,
            lottoTypes: [
                { id: 1, type: 'Hourly Lotto' },
                { id: 2, type: 'Two Hour Lotto' },
                { id: 3, type: 'Six Hour Lotto' },
                { id: 4, type: 'Twelve Hour Lotto' },
                { id: 5, type: 'Daily Lotto' },
                { id: 6, type: 'Weekly Lotto' },
            ],
            ticketsBought: null,
            winningsTotalPulse: null,
        };
    },
  components: {
    Lotto,
    //   MetaMaskSDK,
  },
  methods: {
    InitializePage() {
        this.GetLottoTimes();
    },
    async GetWinnings(val) {
        let payload = { AccountNum: val };

        await fetch('api/NotALottery/GetWinnings', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.winningsTotalPulse = json;
            } else {
                this.winningsTotalPulse = null;
            }
            return;
        });
    },
    async GetLottoTimes() {
        // this.lottoTimes = null;
        await fetch('api/NotALottery/GetLottoTimes', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.lottoTimes = json;
            } else {
                this.lottoTimes = null;
            }
            return;
        });
    },
    async GetTicketsBought(val) {
        let payload = { AccountNum: val };

        await fetch('api/NotALottery/GetTicketsBought', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.ticketsBought = json;
            } else {
                this.ticketsBought = null;
            }
            return;
        });
    },
    async GetWinnerLists(val) {
        let payload = { AccountNum: val };

        await fetch('api/NotALottery/GetWinnerLists', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.winnerLists = json;
            } else {
                this.winnerLists = null;
            }
            return;
        });
    },
    async ConnectMetaMask() {
        const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' })
            .catch((err) => {
            if (err.code === 4001) {
                // EIP-1193 userRejectedRequest error
                // If this happens, the user rejected the connection request.
                console.log('Please connect to MetaMask.');
            } else {
                console.error(err);
            }
            });
        const account = accounts[0];
        this.accountNum = account;
    },
    async ClaimWinnings() {
        let payload = { AccountNum: this.accountNum };

        await fetch('api/NotALottery/ClaimWinnings', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.winningsTotalPulse = json;
            } else {
                this.winningsTotalPulse = null;
            }
            return;
        });
    },
    async RefreshLotto() {
        this.GetWinnings(this.accountNum);
        // this.lottoTimes = null;
        // this.winnerLists = null;
        this.GetLottoTimes();
        this.GetTicketsBought(this.accountNum);
        this.GetWinnerLists(this.accountNum);
    },
  },
  watch: {
    eth: function () {
        let vm = this;
        setTimeout(() => {
            this.accountNum = vm.accountNum;
            this.GetTicketsBought(vm.accountNum);
            this.GetWinnings(vm.accountNum);
        }, 1000)
        // this.connected = window.ethereum._state.accounts.length;
        // this.connected = window.ethereum.request({method: 'eth_accounts'});
    }
  },
  created() {
    this.eth = ethereum;

    setTimeout(() => {
        this.accountNum = ethereum.selectedAddress;
    }, 1000)


    // if (provider) {
    //     // From now on, this should always be true:
    //     // provider === window.ethereum
    //     startApp(provider); // initialize your app
    // } else {
    //     console.log('Please install MetaMask!');
    // }

    // function startApp(provider) {
    //     // If the provider returned by detectEthereumProvider isn't the same as
    //     // window.ethereum, something is overwriting it � perhaps another wallet.
    //     if (provider !== window.ethereum) {
    //         console.error('Do you have multiple wallets installed?');
    //     }
    //     // console.log("provider is on" + provider);
    //     // Access the decentralized web!
    // }
    

    // const chainId = window.ethereum.request({ method: 'eth_chainId' });
    // console.log(chainId);

    // window.ethereum.on('chainChanged', handleChainChanged);

    // function handleChainChanged(chainId) {
    // // We recommend reloading the page, unless you must do otherwise.
    // window.location.reload();
    // console.log(chainId);
    // }
    
  },
  mounted() {
    this.InitializePage();
  },
}
</script>

<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  height: 100vh;
}
.center {
    text-align: center;
}
.page-header {
    margin-top: 15vh;
    color: white;
}
.connect-btn {
    border: 1px solid white !important;
    font-size: 12px;
}
.connected-btn {
    border: 1px solid white !important;
}
.vue-pages {
    height: 100vh;
    padding-top:56px;
    overflow: hidden;
}
.pointer {
    cursor: pointer;
}
</style>
