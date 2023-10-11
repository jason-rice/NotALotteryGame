<template>
    <div class="center">
        <nav class="navbar navbar-expand-sm bg-dark navbar-dark fixed-top">
            <div class="container-fluid">
                <a class="navbar-brand" href="https://localhost:5002/">Not A Lottery</a>
                <button v-if="accountNum === null" @click="ConnectMetaMask()" type="button" class="btn btn-dark connect-btn">Connect MetaMask</button>
                <button v-else type="button" class="btn btn-dark connected-btn">{{ accountNumDisplay }}</button>
            </div>
        </nav>
        
        <div class="page-header">
            <h3>Invite your friends! The more that play, the bigger the prize!</h3>
            <br>
            <p>Buy tickets for one or more of the six different drawings. Each drawing has it's own prize!</p>
            <br />
            <h5 v-if="winningsTotalPulse !== null && winningsTotalPulse > 0">You've won {{ winningsTotalPulse }} of PLS!!! Congrats!!!</h5>
            <div><button type="button" class="btn btn-success" @click="ClaimWinnings()" :disabled="winningsTotalPulse === null || winningsTotalPulse === 0">Claim All Winnings!!!</button></div>
            <br />
        </div>

        <div class="row row-cols-1 row-cols-md-3 g-4" style="padding: 5% 10%;" v-if="accountNum !== null && lottoTypes !== null && lottoTimes !== null && ticketsBought !== null && totalTicketsBought !== null && totalPlsList !== null && winnerLists !== null">
            <Lotto  
                v-for="(t, index) in lottoTypes" :key="t"
                :lottoType="lottoTypes[index]" 
                :timeEnd="lottoTimes[index].dateAndTime"
                :ticketsBoughtIncoming="ticketsBought[index]"
                :totalTicketsBoughtIncoming="totalTicketsBought[index]"
                :totalPlsIncoming="totalPlsList[index]"
                :winners="winnerLists[index]"
                @refreshLotto="RefreshLotto()"
            />
        </div>
        <div class="row row-cols-1 row-cols-md-3 g-4" style="padding: 5% 10%;" v-if="accountNum === null && lottoTypes !== null && lottoTimes !== null && totalTicketsBought && totalPlsList !== null && winnerLists !== null">
            <Lotto  
                v-for="(t, index) in lottoTypes" :key="t"
                :lottoType="lottoTypes[index]" 
                :timeEnd="lottoTimes[index].dateAndTime"
                :ticketsBoughtIncoming="0"
                :totalTicketsBoughtIncoming="totalTicketsBought[index]"
                :totalPlsIncoming="totalPlsList[index]"
                :winners="winnerLists[index]"
                @refreshLotto="RefreshLotto()"
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
    // window.ethereum.on('chainChanged', (chainId) => this.reload(chainId));
    
export default {
  name: 'App',
  data() {
        return {
            accountNum: null,
            accountNumDisplay: null,
            eth: null,
            chainId: null,
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
            totalTicketsBought: null,
            totalPlsList: null,
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
        this.GetWinnerLists();
        this.GetTotalTicketsBought();
        this.GetTotalPlsList();
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
    async GetLottoTimes() {
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
    async GetWinnerLists() {
        await fetch('api/NotALottery/GetWinnerLists', {
            method: 'Get',
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
    async GetTotalTicketsBought() {
        await fetch('api/NotALottery/GetTotalTicketsBought', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            this.totalTicketsBought = json;
            return;
        });
    },
    async GetTotalPlsList() {
        await fetch('api/NotALottery/GetTotalPlsList', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            this.totalPlsList = json;
            return;
        });
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
    async GetTicketsBought(val) {
        let payload = { AccountNum: val };

        await fetch('api/NotALottery/GetTicketsBought', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            this.ticketsBought = json;
            return;
        });
    },
    async ClaimWinnings() {
        if (this.winningsTotalPulse === null || this.winningsTotalPulse === 0) {return;}

        this.winningsTotalPulse = 0;
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
        setTimeout(() => {
            this.GetLottoTimes();
            this.GetWinnerLists();
            this.GetWinnings(this.accountNum);
        }, 5000)
    },
    reload() {
        window.location.reload();
    },
  },
  watch: {
    // ethereum() {
    //     console.log('window.ethereum');
    // },
    eth: function () {
        let vm = this;
        // let connected = window.ethereum.isConnected();
        // console.log(connected);
        // const accounts = ethereum.request({method: 'eth_accounts'});       
        //  if (accounts.length) {
        //     console.log(`You're connected to: ${accounts[0]}`);
        //  } else {
        //     console.log("Metamask is not connected");
        //  }
        // console.log(ethereum);
        // console.log(window.ethereum);
        

        // window.ethereum.on('chainChanged', (chainId) => window.location.reload());
        setTimeout(() => {
            this.accountNum = vm.accountNum;
            if (vm.accountNum !== null) {
                this.accountNumDisplay = vm.accountNum.slice(0, 5) + '...' + vm.accountNum.slice(vm.accountNum.length - 5, vm.accountNum.length);
                this.GetWinnings(vm.accountNum);
                this.GetTicketsBought(vm.accountNum);
                this.chainId = window.ethereum.chainId;
                if (this.chainId !== '0x171') {
                    window.ethereum.request({
                        "method": "wallet_switchEthereumChain",
                        "params": [ { "chainId": "0x171" } ]
                    });
                }
            }
        }, 1000)
        // this.connected = window.ethereum._state.accounts.length;
        // this.connected = window.ethereum.request({method: 'eth_accounts'});
    }
  },
  created() {
    // this.eth = ethereum;

    // setTimeout(() => {
    //     this.accountNum = ethereum.selectedAddress;
    // }, 1000)


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
    this.eth = ethereum;

    setTimeout(() => {
        this.accountNum = ethereum.selectedAddress;
    }, 1000)

    this.InitializePage();

    window.ethereum.on('chainChanged', (chainId) => this.reload(chainId));
    window.ethereum.on('accountsChanged', (account) => console.log('account changed '+account));
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
    margin-top: 20vh;
    color: white;
    padding: 0px 30px;
}
@media (min-width: 768px) { 
    .page-header {
        margin-top: 15vh;
    }
 }
.connect-btn {
    border: 1px solid white !important;
    font-size: 12px;
}
.connected-btn {
    border: 1px solid white !important;
}
</style>
