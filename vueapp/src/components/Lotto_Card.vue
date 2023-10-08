<template>
  <div class="col">
    <div class="card">
      <div class="card-header cards-color-scheme border-white">
        <h5>{{ lottoType.type }}</h5>
      </div>
      <div class="card-body cards-color-scheme body-border">
        <div><p class="card-text">{{ timeLeft }}</p></div>
        <br />
        <div><input type="number" v-model="ticketNum" :min="1" :max="10000" step="1" style="width: 50%" /></div>
        <br />
        <div><button type="button" class="btn btn-success" @click="BuyTickets()" :disabled="(timeLeft === 'EXPIRED')">Buy Ticket(s)</button></div>
        <br />
        <div><p class="card-text">Number of tickets bought: {{ ticketsBought }}</p></div>
        <!-- <br />
        <div><button type="button" class="btn btn-success" @click="Claim()" :disabled="true">Claim</button></div>
        <br />
        <div><button type="button" class="btn btn-success" @click="RunLotto()" :disabled="!(timeLeft === 'EXPIRED')">Run Lotto</button></div> -->
      </div>
      <div class="card-footer cards-color-scheme border-white">
        Last five winners:
        <p v-for="win in winnersList" :key="win.id">{{ win.addressId }} won {{ win.amountPulse }} PLS!!!</p>
      </div>
    </div>
  </div>
</template>


<script>
import MetaMaskSDK from "@metamask/sdk";
const MMSDK = new MetaMaskSDK();
const ethereum = await MMSDK.getProvider(); // You can also access via window.ethereum

export default {
  props: {
    addressNumber: null,
    lottoType: null,
    timeEnd: null,
    ticketsBoughtIncoming: null,
  },
  data() {
    return {
      nextLotteryTime: null,
      timeLeft: null,
      ticketNum: 1,
      ticketsBought: this.ticketsBoughtIncoming,
      winnersList: [],
    };
  },
  methods: {
    InitializePage() {
      this.BeginCountdown(this.timeEnd);
    },
    BeginCountdown(val) {
      let vm = this;
      vm.nextLotteryTime = new Date(val).getTime();

      var x = setInterval(function () {
        if (val !== null && val !== undefined) {
          let a = new Date(
            new Date().toLocaleString("en-US", { timeZone: "America/Chicago" })
          ).getTime();

          var distance = vm.nextLotteryTime - a;

          var days = Math.floor(distance / (1000 * 60 * 60 * 24));
          var hours = Math.floor(
            (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
          );
          var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
          var seconds = Math.floor((distance % (1000 * 60)) / 1000);

          vm.timeLeft = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";

          // If the count down is over, write some text
          if (distance < 0) {
            clearInterval(x);
            vm.timeLeft = "EXPIRED";
            vm.RunLotto();
          }
        }
      }, 1000);
    },
    async SendPulse() {
      let vm = this;
      ethereum
        .request({
          method: "eth_sendTransaction",
          params: [
            {
              from: this.addressNumber,
              to: "0xFfFB14A9090615798a77dcFD926f0A6eb99Fd5CF", // wallet 1
              //to: "0xA6Ab2919659bA1F6A492d813B945ac76bF5b090E", // wallet 2
              //to: "0x1a552c4DDec9E9Fd9103c1174c23ED270E8Eab4D", // wallet 3
              value: Number(20000000000000000000000 * this.ticketNum).toString(16),
            },
          ],
        })
        .then((txHash) => vm.BuyTickets(txHash))
        .catch((error) => console.error(error));
    },
    async BuyTickets(txHash) {
      console.log(txHash);

      if (this.ticketNum > 0 && this.ticketNum <= 10000) {
        let payload = {
          AccountNum: this.addressNumber,
          TicketNum: this.ticketNum,
          Type: this.lottoType.id,
        };

        await fetch("api/NotALottery/BuyTickets", {
          method: "POST",
          body: JSON.stringify(payload),
          headers: { "Content-type": "application/json; charset=UTF-8" },
        })
        .then((r) => r.json())
        .then((json) => {
          this.ticketsBought = json;
          return;
        });
      }
    },
    async Claim(val) {
      console.log("Claim" + val);
    },
    async RunLotto() {
      let vm = this;
      let payload = { Type: this.lottoType.id };

      await fetch("api/NotALottery/RunLotto", {
          method: "POST",
          body: JSON.stringify(payload),
          headers: { "Content-type": "application/json; charset=UTF-8" },
        })
        .then((r) => r.json())
        .then((json) => {
          let output = json;
          if (output.winner !== null) {
            vm.winnersList.push(output.winner)
          }
          vm.$emit("refreshLotto");
          return;
        });

      // if (this.addressNumber === '0x1a552c4ddec9e9fd9103c1174c23ed270e8eab4d') {
      //   await fetch("api/NotALottery/RunLotto", {
      //     method: "POST",
      //     body: JSON.stringify(payload),
      //     headers: { "Content-type": "application/json; charset=UTF-8" },
      //   })
      //   .then((r) => r.json())
      //   .then((json) => {
      //     let output = json;
      //     if (output.winner !== null) {
      //       vm.winnersList.push(output.winner)
      //     }
      //     vm.$emit("refreshLotto");
      //     return;
      //   });
      // }
    },
  },
  watch: {
    ticketNum() {
      if (this.ticketNum === "" || this.ticketNum < 1) {
        this.ticketNum = 1;
      } else if ( this.ticketNum > 10000) {
        this.ticketNum = 10000;
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
</style>
