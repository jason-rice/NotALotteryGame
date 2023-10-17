<template>
    <div class="center">
        <nav class="navbar navbar-expand-sm bg-dark navbar-dark fixed-top">
            <div class="container-fluid">
                <a class="navbar-brand" href="https://localhost:5002/">
                    <img src="smallcoins2.png" style="margin-right: 10px;"> Not A Lottery
                </a>
                <button v-if="currentAddress === null" @click="ConnectMetaMask()" type="button" class="btn btn-dark connect-btn">Connect MetaMask</button>
                <button v-else type="button" class="btn btn-dark connected-btn">{{ currentAddressDisplay }}</button>
            </div>
        </nav>
        
        <div class="page-header" v-show="showPage">
            <div>
                <h3>Invite your friends! The more that play, the bigger the prize!</h3>
                <br>
                <HowToPlay></HowToPlay>
            </div>
            <br>
            <div v-if="metaMaskDownloaded === false || chainId !== pulseChainId || currentAddress === null">
                <h1 style="color:orangered">You must be on the PulseChain network to play this game!</h1>
                <a v-show="metaMaskDownloaded === false" href="https://metamask.io/download/" target="_blank" class="download-mm-btn">Download MetaMask?</a>
                <button v-show="metaMaskDownloaded !== false && currentAddress === null" 
                    type="button" class="btn btn-warning" @click="ConnectMetaMask()">
                    Connect MetaMask
                </button>
                <button v-show="metaMaskDownloaded !== false && currentAddress !== null && chainId !== pulseChainId" 
                    type="button" class="btn btn-warning" @click="AddPulseChainToMetaMaskOrSwitchToPulseChain()">
                    Switch to PulseChain?
                </button>
                <br>
                <br>
                <br>
            </div>
            <div>
                <h1 v-if="showMyPrizeMoney" style="color: lawngreen;">You've won {{ winningsTotalPulse }} of PLS!!! Congrats!!!</h1>
            </div>
            <div>
                <button type="button" class="btn btn-success" @click="ClaimWinnings()" :disabled="canClaimPrize">Claim All Winnings!!!</button>
            </div>
            <br />
        </div>

        <div v-show="showPage">
            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4" style="padding: 5% 10%;" v-if="showLottos">
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
        </div>
        
        <Disclaimer></Disclaimer>
    </div>
</template>

<script>
    import Lotto from './components/Lotto_Card.vue';
    import HowToPlay from './components/HowToPlay.vue';
    import Disclaimer from './components/Disclaimer.vue';
    
export default {
  name: 'App',
  data() {
        return {
            currentAddress: null,
            chainId: null,
            showPage: false,
            pulseChainId: '0x171',
            lottoTimes: null,
            winnerLists: null,
            lottoTypes: [
                { id: 1, type: 'Two Minute Lotto' },
                { id: 2, type: 'Five Minute Lotto' },
                { id: 3, type: 'Thirty Minute Lotto' },
                { id: 4, type: 'Hourly Lotto' },
                { id: 5, type: 'Two Hour Lotto' },
                { id: 6, type: 'Six Hour Lotto' },
                { id: 7, type: 'Twelve Hour Lotto' },
                { id: 8, type: 'Daily Lotto' },
                { id: 9, type: 'Weekly Lotto' },
            ],
            ticketsBought: null,
            totalTicketsBought: null,
            totalPlsList: null,
            winningsTotalPulse: null,
            statistics: null,
        };
    },
  components: {
    Lotto,
    HowToPlay,
    Disclaimer,
  },
  methods: {
    InitializePage() { // don't need currentAddress for these
        this.GetLottoTimes();
        this.GetWinnerLists();
        this.GetTicketsBought(); // returns an array of 0's if no currentAddress
        this.GetTotalTicketsBought();
        this.GetTotalPlsList();
        this.GetStatistics();

        let vm = this;
        setInterval(function () {
            vm.GetTotalPlsList();
        }, 10000);
        
        setInterval(() => {
            this.GetLottoTimes();
        }, 4000)
    },
    async AccountFound() {
        if (window.ethereum !== null && window.ethereum !== undefined) {
            this.currentAddress = window.ethereum.selectedAddress;
            this.chainId = window.ethereum.chainId;

            window.ethereum.on('chainChanged', (chainId) => this.reload(chainId));
            window.ethereum.on('accountsChanged', (account) => this.reload(account));

            if (this.currentAddress !== null) {
                this.GetWinnings(this.currentAddress);
                this.GetTicketsBought(this.currentAddress);
            } 
        }
        this.showPage = true;
    },
    async AddPulseChainToMetaMaskOrSwitchToPulseChain() {
        try {
            await window.ethereum.request({
                "method": "wallet_switchEthereumChain",
                "params": [ { "chainId": this.pulseChainId } ]
            });
        } catch (switchError) {
            // This error code indicates that the chain has not been added to MetaMask.
            console.log('chain has not been added to this MetaMask wallet');
            if (switchError.code === 4902 || switchError.code === -32603) {
                try {
                    await window.ethereum.request({
                        method: 'wallet_addEthereumChain',
                        params: [
                            {
                                "blockExplorerUrls": [
                                    "https://otter.PulseChain.com"
                                ],
                                chainId: this.pulseChainId,
                                chainName: 'PulseChain',
                                rpcUrls: [
                                    'https://rpc.pulsechain.com'
                                ],
                                "nativeCurrency": {
                                    "name": "PLS",
                                    "symbol": "PLS",
                                    "decimals": 18
                                }
                            },
                        ],
                    });
                    this.reload();
                } catch (addError) {
                // handle "add" chain error
                    console.log('handle "add" chain error');
                }
            }
            // handle other "switch" errors
            console.log('handle other "switch" errors');
        }
    },
    async SwitchToPulseChain() {
        await window.ethereum.request({
            "method": "wallet_switchEthereumChain",
            "params": [ { "chainId": this.pulseChainId } ]
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
        this.currentAddress = accounts[0];
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
    async GetStatistics() {
        await fetch('api/NotALottery/GetStatistics', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.statistics = json;
            } else {
                this.statistics = null;
            }
            return;
        });
    },
    async ClaimWinnings() {
        if (this.winningsTotalPulse === null || this.winningsTotalPulse === 0) {return;}

        this.winningsTotalPulse = 0;
        let payload = { AccountNum: this.currentAddress };

        // eslint-disable-next-line no-undef
        confetti({
            particleCount: 5000,
            spread: 1000,
            origin: { y: .6 }
        });

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
            this.GetWinnerLists();
            this.GetTotalTicketsBought();
            this.GetWinnings(this.currentAddress);
        }, 5000)
    },
    reload() {
        window.location.reload();
    },
  },
  computed: {
    metaMaskDownloaded() {
        if (window.ethereum !== null && window.ethereum !== undefined) {
            return true;
        } else {
            return false;
        }
    },
    showMyPrizeMoney() {
        return this.winningsTotalPulse !== null && this.winningsTotalPulse > 0;
    },
    canClaimPrize() {
        return this.chainId !== this.pulseChainId || this.winningsTotalPulse === null || this.winningsTotalPulse === 0;
    },
    showLottos() {
        return this.lottoTypes !== null && this.lottoTimes !== null && this.ticketsBought !== null && this.totalTicketsBought !== null && this.totalPlsList !== null && this.winnerLists !== null;
    },
    currentAddressDisplay() {
        if (this.currentAddress !== null) {
            return this.currentAddress.slice(0, 5) + '...' + this.currentAddress.slice(this.currentAddress.length - 5, this.currentAddress.length);
        } else {
            return null;
        }
    },
  },
  mounted() {
    setTimeout(() => {
        this.AccountFound();
    }, 1000)

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
    margin-top: 20vh;
    color: white;
    padding: 0px 30px;
}
@media (min-width: 768px) { 
    .page-header {
        margin-top: 10vh;
    }
 }
.connect-btn {
    border: 1px solid white !important;
    font-size: 12px;
}
.connected-btn {
    border: 1px solid white !important;
}
.download-mm-btn {
    cursor: pointer;
    color: #000;
    background-color: #ffc107;
    border-color: #ffc107;
    padding: 6px 12px 10px;
    border-radius: 5px;
    text-decoration: none;
}
.download-mm-btn:hover {
    color: #000;
    background-color: #ffca2c;
    border-color: #ffc720;
}
.download-mm-btn:focus {
    --bs-btn-focus-shadow-rgb: 217,164,6;
}
.download-mm-btn:active {
    color: #000;
    background-color: #ffcd39;
    border-color: #ffc720;
    --bs-btn-active-shadow: inset 0 3px 5px rgba(0, 0, 0, 0.125);
}
.download-mm-btn:disabled {
    color: #000;
    --bs-btn-disabled-bg: #ffc107;
    --bs-btn-disabled-border-color: #ffc107;
}
</style>
