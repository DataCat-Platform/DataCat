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

    static ASTPtr Create(
        std::string metricName, std::vector<ASTPtr> tagSelectors)
    {
        return std::make_shared<MetricSelector>(metricName, tagSelectors);
    }

private:
    std::string metricName;
    std::vector<ASTPtr> tagSelectors;
};

}