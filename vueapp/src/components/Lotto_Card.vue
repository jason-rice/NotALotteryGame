<template>
  <div class="col">
    <div class="card">
      <div class="card-header cards-color-scheme border-white">
        <h5>{{ lottoType.type }}</h5>
      </div>
      <div class="card-body cards-color-scheme body-border">
        <div><p class="card-text" style="color:#ffc107">Prize: {{ totalPls }} PLS!!!</p></div>
        <br />
        <div><p class="card-text" :style="timeLeft === 'Calculating winner!!' ? 'color:indianred' : 'color:white'">{{ timeLeft }}</p></div>
        <br />
        <div><input type="number" v-model="ticketNum" :min="1" :max="10000" step="1" style="width: 50%" :disabled="canBuyTickets" /></div>
        <br />
        <div>
          <button type="button" class="btn btn-success" @click="SendPulse()" :disabled="canBuyTickets">Buy Ticket(s)</button>
        </div>
        <br />
        <div><p class="card-text">Tickets bought: {{ ticketsBought }}</p></div>
      </div>
      <div class="card-footer cards-color-scheme border-white">
        Last five winners:
        <div v-for="win in winnersList" :key="win.id" class="winners-list row">
            <div class="left" style="width: 45%;">{{ win.displayString }}</div>
            <div class="right" style="width: 45%;">{{ win.amountPulse }} PLS!!!</div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>

export default {
  props: {
    lottoType: null,
    timeEnd: null,
    ticketsBoughtIncoming: null,
    totalPlsIncoming: null,
    winners: null,
  },
  data() {
    return {
      // currentAddress: window.ethereum.selectedAddress,
      // chainId: window.ethereum.chainId,
      nextLotteryTime: null,
      timeLeft: null,
      ticketNum: 1,
      ticketsBought: this.ticketsBoughtIncoming,
      totalPls: this.totalPlsIncoming,
      winnersList: this.winners,
    };
  },
  methods: {
    InitializePage() {
      this.BeginCountdown(this.timeEnd);
      this.ShowWinnersListStrings();
    },
    async AccountFound() {
        // console.log('metaMaskDownloaded ' + this.metaMaskDownloaded);
        if (window.ethereum !== null && window.ethereum !== undefined) {
          // console.log('accountNum ' + window.ethereum.selectedAddress);
          // console.log('chainId ' + window.ethereum.chainId);
          this.accountNum = window.ethereum.selectedAddress;
          this.chainId = window.ethereum.chainId;
        }
    },
    BeginCountdown(val) {
      let vm = this;
      vm.nextLotteryTime = new Date(val).getTime();

      var x = setInterval(function () {
        if (val !== null && val !== undefined) {
          let a = new Date(new Date().toLocaleString("en-US", { timeZone: "America/Chicago" })).getTime();

          var distance = vm.nextLotteryTime - a;

          var days = Math.floor(distance / (1000 * 60 * 60 * 24));
          var hours = Math.floor(
            (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
          );
          var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
          var seconds = Math.floor((distance % (1000 * 60)) / 1000);

          vm.timeLeft = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";

          // If the count down is over, write some text
          if (distance < 5000) {
            clearInterval(x);
            vm.timeLeft = "Calculating winner!!";
            vm.ticketsBought = 0;
            vm.totalPls = 0;
            vm.$emit("refreshLotto");
          }
        }
      }, 1000);
    },
    async SendPulse() {
      let vm = this;
      window.ethereum
        .request({
          method: "eth_sendTransaction",
          params: [
            {
              from: this.currentAddress,
              // to: "0xFfFB14A9090615798a77dcFD926f0A6eb99Fd5CF", // wallet 1.1
              // to: "0x182Ea006D7ABd3265021d46ef625cA71543529e3", // wallet 1.2
              //to: "0xA6Ab2919659bA1F6A492d813B945ac76bF5b090E", // wallet 2.1
              //to: "0x49d17bec36e0e88e908a9b0c742a6e33f512d34a", // wallet 2.2
              // to: "0x1a552c4DDec9E9Fd9103c1174c23ED270E8Eab4D", // wallet 3.1
              to: "0x2929d460d1e260a2af4e7d51e0a6f25ef61c899f", // wallet 3.2
              value: Number(20000000000000000000000 * this.ticketNum).toString(16),
            },
          ],
        })
        .then((txHash) => vm.BuyTickets(txHash))
        .catch((error) => console.error(error));
    },
    async BuyTickets(txHash) {
      if (this.ticketNum > 0 && this.ticketNum <= 10000) {
        let payload = {
          AccountNum: this.currentAddress,
          TicketNum: this.ticketNum,
          Type: this.lottoType.id,
          TxHash: txHash,
        };

        await fetch("api/NotALottery/BuyTickets", {
          method: "POST",
          body: JSON.stringify(payload),
          headers: { "Content-type": "application/json; charset=UTF-8" },
        })
        .then((r) => r.json())
        .then((json) => {
          if (json.status !== 400) {
              this.ticketsBought = json;
          } else {
              this.ticketsBought = 0;
          }
          return;
        });
      }
    },
    ShowWinnersListStrings() {
      this.winnersList.forEach(winner => {
        winner.displayString = winner.addressId.slice(0, 5) + '...' + winner.addressId.slice(winner.addressId.length - 5, winner.addressId.length);
      });
    }
  },
  watch: {
    ticketNum() {
      if (this.ticketNum === "" || this.ticketNum < 1) {
        this.ticketNum = 1;
      } else if ( this.ticketNum > 10000) {
        this.ticketNum = 10000;
      }
    },
    timeEnd() {
      this.BeginCountdown(this.timeEnd);
    },
    ticketsBoughtIncoming() {
      this.ticketsBought = this.ticketsBoughtIncoming;
    },
    totalPlsIncoming() {
      this.totalPls = this.totalPlsIncoming;
    },
    winners() {
      this.winnersList = this.winners;
      this.ShowWinnersListStrings();
    },
  },
  computed: {
    canBuyTickets() {
      return this.timeLeft === 'Calculating winner!!' || this.currentAddress === null || this.chainId === null || this.chainId !== '0x171';
    },
    currentAddress() {
      if (window.ethereum !== null && window.ethereum !== undefined) {
        return window.ethereum.selectedAddress;
      } else {
        return null;
      }
    },
    chainId() {
      if (window.ethereum !== null && window.ethereum !== undefined) {
        return window.ethereum.chainId;
      } else {
        return null;
      }
    },
  },
  mounted() {
    this.InitializePage();
  },
};
</script>

<style scoped>
.cards-color-scheme {
  background-color: #202022;
  color: white;
}
.border-white {
  border: 1px solid white;
}
.body-border {
  border-left: 1px solid white;
  border-right: 1px solid white;
}
.winners-list {
  font-size: 15px;
  margin: 0px;
}
.left {
  text-align: left;
}
.right {
  text-align: right;
}
.inline {
  display: inline-block;
}
</style>
