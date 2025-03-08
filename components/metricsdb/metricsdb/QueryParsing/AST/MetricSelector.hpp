#pragma once

#include <memory>
#include <metricsdb/QueryParsing/AST/Base.hpp>
#include <metricsdb/QueryParsing/AST/TagSelector.hpp>
#include <string>
#include <vector>

namespace DB::QueryParsing::AST {

class MetricSelector : public Base {
public:
    MetricSelector(
        const std::string& metricName, std::vector<ASTPtr> tagSelectors)
        : metricName(std::move(metricName))
        , tagSelectors(std::move(tagSelectors)) {};

    static ASTPtr create(
        std::string metricName, std::vector<ASTPtr> tagSelectors)
    {
        return std::make_shared<MetricSelector>(metricName, tagSelectors);
    }

    std::string dumpTree() override
    {
        std::string dump = "METRIC[" + metricName + "{";
        for (auto& tagSelector : tagSelectors) {
            dump += tagSelector->dumpTree();
        }
        dump += "}]";
        return dump;
    }

private:
    std::string metricName;
    std::vector<ASTPtr> tagSelectors;
};

}