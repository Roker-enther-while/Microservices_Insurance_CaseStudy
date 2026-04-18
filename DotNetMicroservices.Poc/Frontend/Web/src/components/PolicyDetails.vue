<template>
    <div>
        <h3>Policy details: {{ policy.number }}</h3>
        <div>
            <div class="row">
                <span><strong>From:</strong> {{ policy.dateFrom }}</span>
            </div>
            <div class="row">
                <span><strong>To:</strong> {{ policy.dateTo }}</span>
            </div>
            <div class="row">
                <span><strong>Policy holder:</strong> {{ policy.policyHolder }}</span>
            </div>
            <div class="row">
                <span><strong>Total premium:</strong> {{ policy.totalPremium }} EUR</span>
            </div>
            <div class="row">
                <span><strong>Product code:</strong> {{ policy.productCode }}</span>
            </div>
            <div class="row">
                <span><strong>Account number:</strong> {{ policy.accountNumber }}</span>
            </div>
            <div class="row">
                <span><strong>Covers:</strong> {{ policy.covers | join }}</span>
            </div>
            <div class="row" style="margin-top: 12px;">
                <button type="button"
                        class="btn btn-primary"
                        v-on:click.stop.prevent="fetchDocuments">
                    📄 Documents
                </button>
            </div>
            <div v-if="documentsList.length > 0" class="row" style="margin-top: 10px;">
                <ul>
                    <li v-for="doc in documentsList" :key="doc">
                        <a :href="documentsBaseUrl + '/' + doc" target="_blank" download>{{ doc }}</a>
                    </li>
                </ul>
            </div>
            <div v-if="noDocuments" class="row" style="margin-top: 10px;">
                <span class="text-muted">No documents available yet for this policy.</span>
            </div>
        </div>
    </div>
</template>

<script>
    import {HTTP} from "./http/ApiClient";

    export default {
        name: "PolicyDetails",
        props: {
            policyNumber: String
        },
        data() {
            return {
                policy: {},
                documentsList: [],
                noDocuments: false,
                documentsBaseUrl: '/api/Documents'
            }
        },
        created: function () {
            HTTP.get("policies/" + this.policyNumber).then(response => {
                this.policy = response.data.policy;
            })
        },
        filters: {
            join: function (value) {
                if (!value)
                    return '';
                return value.join(', ');
            }
        },
        methods: {
            fetchDocuments: function () {
                const policyNum = this.policyNumber;
                HTTP.get("Documents").then(response => {
                    const allDocs = response.data;
                    this.documentsList = allDocs.filter(f => f.includes('Policy_' + policyNum));
                    this.noDocuments = this.documentsList.length === 0;
                }).catch(() => {
                    this.noDocuments = true;
                });
            }
        }
    }
</script>

<style scoped>
</style>

